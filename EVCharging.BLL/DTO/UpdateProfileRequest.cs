namespace EVCharging.BLL.DTO
{
    public class UpdateProfileRequest
    {
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public int? HomeStationId { get; set; } // cho phép chọn Home Station
    }
}