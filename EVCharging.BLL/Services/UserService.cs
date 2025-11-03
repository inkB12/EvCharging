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
            // Validate cơ bản
            if (string.IsNullOrWhiteSpace(req.Email) ||
                string.IsNullOrWhiteSpace(req.Password) ||
                string.IsNullOrWhiteSpace(req.FullName) ||
                string.IsNullOrWhiteSpace(req.Phone))
            {
                return (false, "Thiếu thông tin bắt buộc (email, password, fullName, phone).", null);
            }
            if (req.Password.Length < 6)
                return (false, "Mật khẩu tối thiểu 6 ký tự.", null);

            // Chuẩn hóa email
            var email = NormalizeEmail(req.Email);

            // Check tồn tại
            var existed = await _repo.GetByEmailAsync(email);
            if (existed != null) return (false, "Email đã tồn tại.", null);

            // Tạo thực thể
            var entity = new User
            {
                Email = email,
                Phone = req.Phone.Trim(),
                FullName = req.FullName.Trim(),
                Password = HashSHA256(req.Password), // GIỮ NGUYÊN: SHA256 base64
                Role = "User",                        // mặc định
                IsDeleted = false                     // 0
            };

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();

            return (true, "Đăng ký thành công.", ToDto(entity));
        }

        public async Task<(bool ok, string msg, UserDTO? user)> LoginAsync(LoginRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
                return (false, "Thiếu email hoặc mật khẩu.", null);

            var email = NormalizeEmail(req.Email);
            var acc = await _repo.GetByEmailAsync(email);
            if (acc == null) return (false, "Tài khoản không tồn tại.", null);

            if (acc.IsDeleted) return (false, "Tài khoản đã bị khóa.", null);

            var inputHash = HashSHA256(req.Password); // GIỮ NGUYÊN: SHA256 base64
            if (!FixedTimeEquals(inputHash, acc.Password))
                return (false, "Sai mật khẩu.", null);

            return (true, "OK", ToDto(acc));
        }

        public async Task<UserDTO?> GetByIdAsync(int id)
            => (await _repo.GetByIdAsync(id)) is User u ? ToDto(u) : null;

        private static UserDTO ToDto(User u) => new()
        {
            Id = u.Id,
            Email = u.Email,
            Phone = u.Phone,
            FullName = u.FullName,
            Role = u.Role,
            IsDeleted = u.IsDeleted,
            ServicePlanId = u.ServicePlanId,
            HomeStationId = u.HomeStationId
        };

        private static string NormalizeEmail(string email)
            => email.Trim().ToLowerInvariant();

        // === Hash kiểu SHA256 base64 như bạn yêu cầu (giữ nguyên) ===
        public static string HashSHA256(string input)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(sha.ComputeHash(bytes));
        }

        // So sánh hằng thời gian để hạn chế timing attacks
        private static bool FixedTimeEquals(string a, string b)
        {
            if (a is null || b is null) return false;
            var ba = Encoding.UTF8.GetBytes(a);
            var bb = Encoding.UTF8.GetBytes(b);
            if (ba.Length != bb.Length) return false;
            var diff = 0;
            for (int i = 0; i < ba.Length; i++) diff |= ba[i] ^ bb[i];
            return diff == 0;
        }
    }
}
