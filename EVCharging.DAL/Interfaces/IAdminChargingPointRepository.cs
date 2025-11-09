using EVCharging.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.DAL.Interfaces
{
    public interface IAdminChargingPointRepository
    {
        Task<List<ChargingPoint>> GetAllAsync();
        Task<ChargingPoint?> GetByIdAsync(int id);
        Task AddAsync(ChargingPoint point);
        Task UpdateAsync(ChargingPoint point);
        Task DeleteAsync(int id);

        // Theo dõi trạng thái online/offline
        Task<List<ChargingPoint>> GetPointsByStatusAsync(string status);

        // Điều khiển khởi động/dừng sạc
        Task<bool> SetChargingPointStatusAsync(int id, string status);
    }
}
