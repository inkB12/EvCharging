using EVCharging.BLL.DTO;

namespace EVCharging.BLL.Interfaces
{
    public interface IMapSchedulingService
    {
        Task<List<ChargingStationDto>> GetStationsAsync();
        Task<List<string>> GetPortTypesByStationAsync(int stationId);

        Task<(bool ok, string msg, int? candidatePointId)> CheckAvailabilityAsync(
            int stationId, string portType, DateOnly date, int startHour);

        Task<(bool ok, string msg, int bookingId, int sessionId)> BookAsync(
            int userId, int pointId, DateOnly date, int startHour);
    }
}
