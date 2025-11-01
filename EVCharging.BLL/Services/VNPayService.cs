using System.Net;
using System.Security.Cryptography;
using System.Text;
using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using Microsoft.Extensions.Options;

namespace EVCharging.BLL.Services
{
    public class VNPayService(IOptions<VNPayDTO> settings) : IVNPayService
    {
        private readonly VNPayDTO _settings = settings.Value;

        public string CreateVNPayUrl(int orderId, decimal amount)
        {
            decimal amounts = Convert.ToInt64(amount);
            string orderIds = Guid.NewGuid().ToString().Substring(1) + orderId;

            var vnp_Params = new SortedDictionary<string, string>(StringComparer.Ordinal)
        {
            {"vnp_Version", _settings.Version},
            {"vnp_Command", _settings.Command},
            {"vnp_TmnCode", _settings.TmnCode},
            {"vnp_Locale", "vn"},
            {"vnp_CurrCode", "VND"},
            {"vnp_Amount", ((int)(amounts * 100)).ToString()},
            {"vnp_TxnRef", GetRandomNumber(8)},
            {"vnp_OrderInfo", orderIds},
            {"vnp_OrderType", _settings.OrderType},
            {"vnp_ReturnUrl", _settings.ReturnUrl},
            {"vnp_IpAddr", "128.199.178.23"}
        };

            // 2. Thêm ngày tạo và ngày hết hạn
            var cld = DateTime.Now;
            vnp_Params.Add("vnp_CreateDate", cld.ToString("yyyyMMddHHmmss"));
            vnp_Params.Add("vnp_ExpireDate", cld.AddMinutes(15).ToString("yyyyMMddHHmmss"));

            // 3. Xây dựng HashData (Tương tự logic Java: sắp xếp, nối, URL Encode)
            var hashData = new StringBuilder();
            var query = new StringBuilder();
            foreach (var entry in vnp_Params)
            {
                string fieldName = entry.Key;
                string fieldValue = entry.Value;

                // Cần URL Encode trong C# (sử dụng WebUtility.UrlEncode hoặc HttpUtility.UrlEncode)
                string encodedValue = WebUtility.UrlEncode(fieldValue);

                hashData.Append(fieldName).Append('=').Append(encodedValue);
                query.Append(fieldName).Append('=').Append(encodedValue);

                hashData.Append('&');
                query.Append('&');
            }

            // Loại bỏ ký tự '&' cuối cùng
            string finalHashData = hashData.ToString().TrimEnd('&');
            string finalQuery = query.ToString().TrimEnd('&');

            // 4. Tạo Secure Hash (HMACSHA512)
            string vnp_SecureHash = HmacSHA512(_settings.HashSecret, finalHashData);

            // 5. Tạo URL cuối cùng
            return $"{_settings.BaseUrl}?{finalQuery}&vnp_SecureHash={vnp_SecureHash}";
        }


        private string HmacSHA512(string key, string data)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] messageBytes = Encoding.UTF8.GetBytes(data);

            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashmessage = hmac.ComputeHash(messageBytes);
                foreach (var b in hashmessage)
                {
                    hash.Append(b.ToString("x2"));
                }
            }
            return hash.ToString();
        }

        // Tái tạo logic RandomNumber từ VNPayConfig.java
        private string GetRandomNumber(int len)
        {
            var random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, len)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
