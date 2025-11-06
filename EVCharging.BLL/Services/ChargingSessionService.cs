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
            var entities = await _sessionRepository.GetByStationIdAsync(stationId); // Correct method to fetch a list of sessions
            return entities.Select(s => new ChargingSessionDto
            {
                Id = s.Id,
                UserId = s.Booking.UserId,
                UserFullName = s.Booking.User.FullName,
                PointId = s.PointId,
                PointPortType = s.Point.PortType,
                PointPricePerKwh = s.Point.Price,
                BookTime = (DateTime)s.Booking.StartTime, // Sửa lỗi lần trước
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Status = s.Status,
                EnergyConsumedKwh = s.EnergyConsumedKwh,
                //TotalCost = s.TotalCost
            }).OrderBy(s => s.Status == "Completed")
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

        //public async Task StartSessionAsync(int sessionId)
        //{
        //    var session = await _sessionRepository.GetByIdAsync(sessionId);
        //    if (session == null)
        //    {
        //        throw new Exception("Không tìm thấy phiên sạc");

        //    }
        //    session.Status = "Charging";
        //    session.StartTime = DateTime.UtcNow;
        //    await _sessionRepository.UpdateAsync(session);
        //}

        //public async Task StopSessionAsync(int sessionId, decimal energyConsumed)
        //{
        //    var session = await _sessionRepository.GetByIdAsync(sessionId);
        //    if (session == null)
        //    {
        //        throw new Exception("Không tìm thấy phiên sạc");
        //    }
        //    if (session.Point == null)
        //    {
        //        throw new Exception("Lỗi: Không tìm thấy thông tin điểm sạc để tính tiền.");
        //    }
        //    session.Status = "Completed";
        //    session.EndTime = DateTime.UtcNow;
        //    session.EnergyConsumedKwh = energyConsumed;

        //    // Logic tính tiền (theo yêu cầu của bạn)
        //    session.TotalCost = energyConsumed * session.Point.Price;

        //    await _sessionRepository.UpdateAsync(session);
        //    //
        //}
    }
}
