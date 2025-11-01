using EVCharging.DAL.Entities;

namespace EVCharging.DAL.Interfaces
{
    public interface IChargingSessionRepository
    {
        Task<ChargingSession?> GetByIdAsync(int sessionId);
    }
}
