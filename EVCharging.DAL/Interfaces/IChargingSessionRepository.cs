using EVCharging.DAL.Entities;

namespace EVCharging.DAL.Interfaces
{
    public interface IChargingSessionRepository
    {
        Task<ChargingSession?> GetByIdAsync(int sessionId);
        Task<List<ChargingSession>> GetAllByUserAndYear(int userId, int year);
    }
}
