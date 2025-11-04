namespace EVCharging.BLL.DTO
{
    public class ChargingStationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? Station { get; set; }
        public string Status { get; set; } = null!;
        public decimal Longtitude { get; set; }
        public decimal Latitude { get; set; }
    }
}
