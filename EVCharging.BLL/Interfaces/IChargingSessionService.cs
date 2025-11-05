using EVCharging.BLL.DTO;

namespace EVCharging.BLL.Interfaces
{
    public interface IChargingSessionService
    {
        Task<List<ChargingSessionDto>> GetSessionsByStationAsync(int stationId);
        //Task StartSessionAsync(int sessionId);
        //Task StopSessionAsync(int sessionId, decimal energyConsumed);
    }
}
