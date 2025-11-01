namespace EVCharging.BLL.DTO
{
    public class VNPayDTO
    {
        public string? TmnCode { get; set; }
        public string? HashSecret { get; set; }
        public string? BaseUrl { get; set; }
        public string? ReturnUrl { get; set; }
        public string? Version { get; set; }
        public string? Command { get; set; }
        public string? OrderType { get; set; }
    }
}
