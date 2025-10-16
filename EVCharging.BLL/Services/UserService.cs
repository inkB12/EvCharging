using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using EVCharging.DAL.Entities;
using EVCharging.DAL.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace EVCharging.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        public UserService(IUserRepository repo) => _repo = repo;

        public async Task<(bool ok, string msg, UserDTO? user)> RegisterAsync(RegisterRequest req)
        {
            var existed = await _repo.GetByEmailAsync(req.Email);
            if (existed != null) return (false, "Email đã tồn tại.", null);

            var entity = new User
            {
                Email = req.Email.Trim(),
                Password = HashSHA256(req.Password),
                FullName = req.FullName?.Trim(),
                Role = "User",
                Status = 1 // 1=active
            };
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();

            return (true, "Đăng ký thành công.", ToDto(entity));
        }

        public async Task<(bool ok, string msg, UserDTO? user)> LoginAsync(LoginRequest req)
        {
            var acc = await _repo.GetByEmailAsync(req.Email);
            if (acc == null) return (false, "Tài khoản không tồn tại.", null);
            if (acc.Status == 0) return (false, "Tài khoản đã bị vô hiệu.", null);

            if (HashSHA256(req.Password) != acc.Password)
                return (false, "Sai mật khẩu.", null);

            return (true, "OK", ToDto(acc));
        }

        public async Task<UserDTO?> GetByIdAsync(int id)
            => (await _repo.GetByIdAsync(id)) is User u ? ToDto(u) : null;

        private static UserDTO ToDto(User u) => new()
        {
            Id = u.Id,
            Email = u.Email,
            FullName = u.FullName,
            Role = u.Role
        };

        // === Hash kiểu SHA256 base64 như bạn yêu cầu ===
        public static string HashSHA256(string input)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(sha.ComputeHash(bytes));
        }
    }
}
