using EVCharging.DAL.Entities;

namespace EVCharging.DAL.Interfaces
{
    public interface IAdminChargingStationRepository
    {
        Task<List<ChargingStation>> GetAllAsync();
        Task<ChargingStation?> GetByIdAsync(int id);
        Task<ChargingStation> AddAsync(ChargingStation station);
        Task UpdateAsync(ChargingStation station);
        Task DeleteAsync(int id);

        // Lấy danh sách trạm có điểm sạc kèm theo
        Task<List<ChargingStation>> GetStationsWithPointsAsync();

        // Cập nhật trạng thái hoạt động của trạm
        Task UpdateStationStatusAsync(int id, string status);
    }
}


