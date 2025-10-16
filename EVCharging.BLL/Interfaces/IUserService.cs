using EVCharging.BLL.DTO;

namespace EVCharging.BLL.Interfaces
{
    public interface IUserService
    {
        Task<(bool ok, string msg, UserDTO? user)> RegisterAsync(RegisterRequest req);
        Task<(bool ok, string msg, UserDTO? user)> LoginAsync(LoginRequest req);
        Task<UserDTO?> GetByIdAsync(int id);
    }
}
