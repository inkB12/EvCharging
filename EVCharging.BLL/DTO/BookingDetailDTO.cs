namespace EVCharging.BLL.DTO
{
    public class BookingDetailDTO
    {
        public int BookingId { get; set; }
        public DateTime? StartTime { get; set; }   // UTC
        public DateTime? EndTime { get; set; }     // UTC
        public decimal Price { get; set; }
        public string Status { get; set; } = null!;

        public List<SessionItemDTO> Sessions { get; set; } = new();
    }

    public class SessionItemDTO
    {
        public int SessionId { get; set; }
        public string Status { get; set; } = null!;
        public DateTime? StartTime { get; set; }   // UTC - có thể null (đặt mới)
        public DateTime? EndTime { get; set; }     // UTC - có thể null
        public decimal? EnergyConsumedKwh { get; set; }

        public int PointId { get; set; }
        public string PortType { get; set; } = "";
        public string StationName { get; set; } = "";
        public string? StationLocation { get; set; }
    }
}
