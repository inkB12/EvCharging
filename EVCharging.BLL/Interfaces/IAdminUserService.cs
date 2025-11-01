using EVCharging.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
