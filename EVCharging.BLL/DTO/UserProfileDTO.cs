namespace EVCharging.BLL.DTO
{
    public class UserProfileDTO
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public int? HomeStationId { get; set; }
        public string? Role { get; set; } // xem, không cho sửa từ trang user
    }
}