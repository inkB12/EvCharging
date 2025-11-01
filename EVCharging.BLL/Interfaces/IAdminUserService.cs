using EVCharging.BLL.DTO;

namespace EVCharging.BLL.Interfaces
{
    public interface IAdminUserService
    {
        Task<List<AdminUserDto>> GetAllAsync();
        Task<AdminUserDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(AdminUserDto dto);
        Task UpdateAsync(AdminUserDto dto);
        Task DeleteAsync(int id);
        Task<List<AdminUserDto>> GetByRoleAsync(string role);
        Task<int> CountByRoleAsync(string role);
    }
}
