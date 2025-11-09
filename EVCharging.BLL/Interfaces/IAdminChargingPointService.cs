using EVCharging.BLL.AdminDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.BLL.Interfaces
{
    public interface IAdminChargingPointService
    {
        Task<List<AdminChargingPointDTO>> GetAllAsync();
        Task<AdminChargingPointDTO?> GetByIdAsync(int id);
        Task AddAsync(AdminChargingPointDTO dto);
        Task UpdateAsync(AdminChargingPointDTO dto);
        Task DeleteAsync(int id);
        Task<List<AdminChargingPointDTO>> GetByStatusAsync(string status);
        Task<bool> SetPointStatusAsync(int id, string status);
    }
}
