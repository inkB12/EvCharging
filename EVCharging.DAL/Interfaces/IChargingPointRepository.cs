

using EVCharging.DAL.Entities;

namespace EVCharging.DAL.Interfaces
{
     public interface IChargingPointRepository
    {
        Task<List<ChargingPoint>> GetPointsByStationAsync(int stationId);
        Task<ChargingPoint?> GetByIdAsync(int id);
        Task<int> CreateAsync(ChargingPoint entity);
        Task UpdateAsync(ChargingPoint entity);
        Task DeleteAsync(int id);

        // Thống kê và điều khiển
        Task<int> CountPointsByStatusAsync(string status);
        Task<List<ChargingPoint>> GetPointsByStatusAsync(string status);
    }
}
