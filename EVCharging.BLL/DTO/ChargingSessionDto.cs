namespace EVCharging.BLL.DTO
{
    public class ChargingSessionDto
    {
        public int Id { get; set; }

        // Thông tin User (từ Booking)
        public int UserId { get; set; }
        public string UserFullName { get; set; }

        // Thông tin Điểm sạc
        public int PointId { get; set; }
        public string PointPortType { get; set; }
        public decimal PointPricePerKwh { get; set; }

        // Thông tin Phiên sạc
        public int BookingId { get; set; }
        public DateTime BookTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Status { get; set; }
        public decimal? EnergyConsumedKwh { get; set; }
        public decimal? TotalCost { get; set; }
    }
}
