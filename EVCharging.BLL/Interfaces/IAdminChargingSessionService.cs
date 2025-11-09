using EVCharging.BLL.AdminDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.BLL.Interfaces
{
    public interface IAdminChargingSessionService
    {
        Task<List<AdminChargingSessionDTO>> GetAllAsync();
        Task<int> CountAllAsync();
        Task<int> CountByStatusAsync(string status);
    }
}
