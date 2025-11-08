using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.DTO;

namespace EVCharging.BLL.Interfaces
{
    public interface IAdminUserService
    {
        Task<List<AdminUserDTO>> GetAllAsync();
        Task<AdminUserDTO?> GetByIdAsync(int id);
        Task AddAsync(AdminUserDTO dto);
        Task UpdateAsync(AdminUserDTO dto);
        Task DeleteAsync(int id);
        Task<List<AdminUserDTO>> GetByRoleAsync(string role);
        Task<bool> UpdateUserRoleAsync(int id, string newRole);
    }
}
