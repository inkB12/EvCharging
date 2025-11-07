using EVCharging.BLL.AdminDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.BLL.Interfaces
{
    public interface IAdminChargingStationService
    {
        Task<List<AdminChargingStationDTO>> GetAllAsync();
        Task<AdminChargingStationDTO?> GetByIdAsync(int id);
        Task AddAsync(AdminChargingStationDTO dto);
        Task UpdateAsync(AdminChargingStationDTO dto);
        Task DeleteAsync(int id);
        Task UpdateStatusAsync(int id, string status);
    }
}
