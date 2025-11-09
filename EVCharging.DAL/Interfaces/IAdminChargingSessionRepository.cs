using EVCharging.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.DAL.Interfaces
{
    public interface IAdminChargingSessionRepository
    {
        Task<int> CountAllAsync();
        Task<int> CountByStatusAsync(string status);
        Task<List<ChargingSession>> GetAllAsync();
    }
}
