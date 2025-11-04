using EVCharging.DAL.Entities;

namespace EVCharging.DAL.Interfaces
{
    public interface IChargingSessionRepository
    {
        Task<ChargingSession?> GetByIdAsync(int sessionId);
        Task<List<ChargingSession>> GetAllByUserAndYear(int userId, int year);
        Task<List<int>> GetBusyPointIdsAsync(List<int> pointIds, DateTime start, DateTime end);
        Task<bool> HasConflictAsync(int pointId, DateTime start, DateTime end);
        Task<int> CreateAsync(ChargingSession entity);
    }
}
