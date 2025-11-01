using System.Text.Json.Serialization;

namespace EVCharging.BLL.DTO
{
    public class MomoResponseDTO
    {
        [JsonPropertyName("requestId")]
        public string? RequestId { get; set; }
        [JsonPropertyName("resultCode")]
        public int ResultCode { get; set; }
        [JsonPropertyName("orderId")]
        public string? OrderId { get; set; }
        [JsonPropertyName("message")]
        public string? Message { get; set; }
        [JsonPropertyName("payUrl")]
        public string? PayUrl { get; set; }
        [JsonPropertyName("qrCodeUrl")]
        public string? QrCodeUrl { get; set; }
        [JsonPropertyName("deepLink")]
        public string? DeepLink { get; set; }
    }
}
