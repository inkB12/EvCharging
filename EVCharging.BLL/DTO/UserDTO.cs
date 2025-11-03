namespace EVCharging.BLL.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? FullName { get; set; }
        public string? Role { get; set; }
        public bool IsDeleted { get; set; }
        public int? ServicePlanId { get; set; }
        public int? HomeStationId { get; set; }
    }
}
