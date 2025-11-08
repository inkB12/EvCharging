using EVCharging.BLL.AdminDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.BLL.Interfaces
{
    public interface IAdminServicePlanService
    {
        Task<List<AdminServicePlanDTO>> GetAllAsync();
        Task<AdminServicePlanDTO?> GetByIdAsync(int id);
        Task AddAsync(AdminServicePlanDTO dto);
        Task UpdateAsync(AdminServicePlanDTO dto);
        Task DeleteAsync(int id);
    }
}
