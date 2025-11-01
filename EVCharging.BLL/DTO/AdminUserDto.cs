namespace EVCharging.BLL.DTO
{
    public class AdminUserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? FullName { get; set; }
        public string? Role { get; set; }
        public string? Vehicle { get; set; }
        public bool IsDeleted { get; set; }
        public int? ServicePlanId { get; set; }
        public int? HomeStationId { get; set; }
    }
}
