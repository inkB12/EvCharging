

using EVCharging.DAL.Entities;

namespace EVCharging.DAL.Interfaces
{
    public interface IChargingStationRepository
    {
        Task<List<ChargingStation>> GetAllStationsAsync();
        Task<ChargingStation?> GetByIdAsync(int id);
        Task<int> CreateAsync(ChargingStation entity);
        Task UpdateAsync(ChargingStation entity);
        Task DeleteAsync(int id);

        // Theo dõi trạng thái trạm và điểm sạc
        Task<List<ChargingStation>> GetStationsWithPointsAsync();

        // Báo cáo tổng hợp
        Task<int> CountOnlineStationsAsync();
        Task<int> CountOfflineStationsAsync();
    }
}
