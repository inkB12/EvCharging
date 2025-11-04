namespace EVCharging.BLL.DTO
{
    public class BookingListItemDTO
    {
        public int BookingId { get; set; }
        public string StationName { get; set; } = "";
        public string? StationLocation { get; set; }
        public string PortType { get; set; } = "";
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Status { get; set; } = "";     // success | cancelled | ongoing
        public decimal Price { get; set; }           // hiện đang 0, staff xử lý sau
    }
}
