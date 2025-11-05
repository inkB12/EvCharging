using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using EVCharging.DAL.Entities;
using EVCharging.DAL.Interfaces;

namespace EVCharging.BLL.Services
{
    public class MapSchedulingService : IMapSchedulingService
    {
        private readonly IChargingStationRepository _stationRepo;
        private readonly IChargingPointRepository _pointRepo;
        private readonly IChargingSessionRepository _sessionRepo;
        private readonly IBookingRepository _bookingRepo;

        public MapSchedulingService(
            IChargingStationRepository stationRepo,
            IChargingPointRepository pointRepo,
            IChargingSessionRepository sessionRepo,
            IBookingRepository bookingRepo)
        {
            _stationRepo = stationRepo;
            _pointRepo = pointRepo;
            _sessionRepo = sessionRepo;
            _bookingRepo = bookingRepo;
        }

        public async Task<List<ChargingStationDto>> GetStationsAsync()
        {
            var stations = await _stationRepo.GetAllStationsAsync();
            return stations.Select(s => new ChargingStationDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Location = s.Location,
                Station = s.Station,
                Status = s.Status,
                Longtitude = s.Longtitude,
                Latitude = s.Latitude
            }).ToList();
        }

        public async Task<List<string>> GetPortTypesByStationAsync(int stationId)
        {
            var points = await _pointRepo.GetPointsByStationAsync(stationId);
            if (points == null || points.Count == 0) return new List<string>();

            var types = points
                .Where(p => p.Status != null && p.Status.Equals("online", StringComparison.OrdinalIgnoreCase))
                .Select(p => p.PortType?.Trim() ?? "")
                .Where(t => t == "AC" || t == "CCS" || t == "CHAdeMO")
                .Distinct()
                .OrderBy(t => t)
                .ToList();

            return types;
        }

        public async Task<(bool ok, string msg, int? candidatePointId)> CheckAvailabilityAsync(
            int stationId, string portType, DateOnly date, int startHour)
        {
            // User chọn giờ LOCAL -> chuyển UTC để lưu/so sánh trong DB
            var local = new DateTime(date.Year, date.Month, date.Day, startHour, 0, 0, DateTimeKind.Local);
            var startUtc = local.ToUniversalTime();
            var endUtc = startUtc.AddHours(1);

            var points = await _pointRepo.GetPointsByStationAsync(stationId);
            var candidates = points
                .Where(p => p.Status.Equals("online", StringComparison.OrdinalIgnoreCase)
                         && p.PortType.Equals(portType, StringComparison.OrdinalIgnoreCase))
                .Select(p => p.Id)
                .ToList();

            if (!candidates.Any())
                return (false, "Không có Charging Point ONLINE cho Port Type này.", null);

            var busyPointIds = await _sessionRepo.GetBusyPointIdsAsync(candidates, startUtc, endUtc);
            var freeId = candidates.Except(busyPointIds).FirstOrDefault();

            if (freeId == 0)
                return (false, "Khung giờ đã kín cho Port Type này.", null);

            return (true, "Khung giờ còn trống.", freeId);
        }

        public async Task<(bool ok, string msg, int bookingId, int sessionId)> BookAsync(
            int userId, int pointId, DateOnly date, int startHour)
        {
            // User chọn giờ LOCAL -> chuyển UTC
            var local = new DateTime(date.Year, date.Month, date.Day, startHour, 0, 0, DateTimeKind.Local);
            var startUtc = local.ToUniversalTime();
            var endUtc = startUtc.AddHours(1);

            // check trùng slot dựa trên thời gian UTC
            var conflict = await _sessionRepo.HasConflictAsync(pointId, startUtc, endUtc);
            if (conflict) return (false, "Khung giờ vừa được người khác đặt.", 0, 0);

            // Booking: lưu UTC
            var booking = new Booking
            {
                UserId = userId,
                BookingTime = DateTime.UtcNow,
                StartTime = startUtc,
                EndTime = endUtc,
                Price = 0,
                Status = "ongoing"
            };
            var bookingId = await _bookingRepo.CreateAsync(booking);

            // Session: StartTime = Booking.StartTime (UTC), EndTime null
            var session = new ChargingSession
            {
                BookingId = bookingId,
                PointId = pointId,
                StartTime = startUtc,
                EndTime = null,
                EnergyConsumedKwh = null,
                Status = "coming-soon"
            };
            var sessionId = await _sessionRepo.CreateAsync(session);

            return (true, "Đặt lịch thành công.", bookingId, sessionId);
        }
    }
}
