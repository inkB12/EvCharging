using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using EVCharging.DAL.Entities;
using EVCharging.DAL.Interfaces;

namespace EVCharging.BLL.Services
{
    public class ChargingSessionService : IChargingSessionService
    {
        private readonly IChargingSessionRepository _sessionRepository;
        public ChargingSessionService(IChargingSessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public async Task<ChargingSessionDto> GetByIdAsync(int id)
        {
            return Map(await _sessionRepository.GetByIdAsync(id));
        }

        public async Task<List<ChargingSessionDto>> GetSessionsByStationAsync(int stationId)
        {
            var entities = await _sessionRepository.GetByStationIdAsync(stationId);
            return entities.Select(s => new ChargingSessionDto
            {
                Id = s.Id,
                UserId = s.Booking.UserId,
                UserFullName = s.Booking.User.FullName,
                PointId = s.PointId,
                PointPortType = s.Point.PortType,
                PointPricePerKwh = s.Point.Price,
                BookTime = (DateTime)s.Booking.StartTime,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Status = s.Status,
                EnergyConsumedKwh = s.EnergyConsumedKwh,
            })
            .OrderBy(s => s.Status == "Completed")
            .ThenByDescending(s => s.StartTime ?? s.BookTime)
            .ToList();
        }

        private ChargingSessionDto Map(ChargingSession session)
        {
            if (session == null) return null;

            return new ChargingSessionDto
            {
                EndTime = session.EndTime,
                EnergyConsumedKwh = session.EnergyConsumedKwh,
                Id = session.Id,
                PointId = session.PointId,
                Status = session.Status,
                StartTime = session.StartTime,
                BookingId = session.Booking.Id,
            };
        }
    }
}
