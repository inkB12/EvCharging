namespace EVCharging.BLL.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string? FullName { get; set; }
        public string? Role { get; set; }
    }
}
