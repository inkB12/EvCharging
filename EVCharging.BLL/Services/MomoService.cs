using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using Microsoft.Extensions.Options;

namespace EVCharging.BLL.Services
{
    public class MomoService(IOptions<MomoOptionDTO> options) : IMomoService
    {
        private readonly IOptions<MomoOptionDTO> _options = options;
        private static readonly HttpClient _client = new();

        public async Task<MomoResponseDTO> CreatePaymentAsync(int orderId, decimal orderTotal)
        {
            string requestIds = DateTime.UtcNow.Ticks.ToString();
            decimal amounts = Convert.ToInt64(orderTotal);
            string orderIds = Guid.NewGuid().ToString().Substring(1) + orderId;

            var rawData =
                $"accessKey={_options.Value.AccessKey}" +
                $"&amount={amounts}" +
                $"&extraData=" +
                $"&ipnUrl={_options.Value.IpnUrl}" +
                $"&orderId={orderIds}" +
                $"&orderInfo=pay with MoMo" +
                $"&partnerCode={_options.Value.PartnerCode}" +
                $"&redirectUrl={_options.Value.RedirectUrl}" +
                $"&requestId={requestIds}" +
                $"&requestType={_options.Value.RequestType}";

            var signatures = GetSignature(rawData, _options.Value.SecretKey);

            var requestData = new
            {
                orderInfo = "pay with MoMo",
                partnerCode = _options.Value.PartnerCode,
                ipnUrl = _options.Value.IpnUrl,
                redirectUrl = _options.Value.RedirectUrl,
                requestType = _options.Value.RequestType,
                orderId = orderIds,
                amount = amounts,
                accessKey = _options.Value.AccessKey,
                requestId = requestIds,
                extraData = "",
                signature = signatures,
            };

            StringContent httpContent = new(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

            var quickPayResponse = await _client.PostAsync("https://test-payment.momo.vn/v2/gateway/api/create", httpContent);

            var contents = await quickPayResponse.Content.ReadAsStringAsync();

            var momoResponse = JsonSerializer.Deserialize<MomoResponseDTO>(contents);

            return momoResponse;
        }

        private static String GetSignature(String text, String key)
        {
            ASCIIEncoding encoding = new();

            Byte[] textBytes = encoding.GetBytes(text);
            Byte[] keyBytes = encoding.GetBytes(key);

            Byte[] hashBytes;

            using (HMACSHA256 hash = new(keyBytes))
                hashBytes = hash.ComputeHash(textBytes);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
