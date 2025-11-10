using System;
using System.Threading.Tasks;
using EVCharging.BLL.Interfaces;
using EVCharging.DAL.Interfaces;

namespace EVCharging.BLL.Services
{
    public class ChargeRuntimeService : IChargeRuntimeService
    {
        private readonly IChargingSessionRepository _sessionRepo;
        private readonly IChargingPointRepository _pointRepo;

        public ChargeRuntimeService(
            IChargingSessionRepository sessionRepo,
            IChargingPointRepository pointRepo)
        {
            _sessionRepo = sessionRepo;
            _pointRepo = pointRepo;
        }

        public async Task<(bool ok, string msg, ChargeSessionRuntimeDTO? dto)> GetSessionAsync(int sessionId)
        {
            var s = await _sessionRepo.GetByIdAsync(sessionId);
            if (s == null) return (false, "Không tìm thấy session.", null);

            var p = s.Point;
            var st = p?.Station;

            DateTime? startUtc = s.StartTime.HasValue ? EnsureUtc(s.StartTime.Value) : (DateTime?)null;
            DateTime? endUtc = s.EndTime.HasValue ? EnsureUtc(s.EndTime.Value) : (DateTime?)null;

            var dto = new ChargeSessionRuntimeDTO
            {
                SessionId = s.Id,
                BookingId = s.BookingId,
                PointId = s.PointId,

                PortType = p?.PortType ?? "",
                PowerLevelKW = p?.PowerLevelKw,        // kWh mục tiêu
                ChargingSpeedKW = p?.ChargingSpeedKw,  // AC: phút để đầy; ngược lại: kW

                StartTime = startUtc,
                EndTime = endUtc,
                Status = s.Status,

                StationName = st?.Name,
                StationLocation = st?.Location,

                EnergyConsumedKwh = s.EnergyConsumedKwh
            };
            return (true, "OK", dto);
        }

        public async Task<(bool ok, string msg)> StartAsync(int sessionId, DateTime startUtc)
        {
            var s = await _sessionRepo.GetByIdAsync(sessionId);
            if (s == null) return (false, "Không tìm thấy session.");

            if (!string.Equals(s.Status, "coming-soon", StringComparison.OrdinalIgnoreCase))
                return (false, $"Không thể bắt đầu, trạng thái hiện tại: {s.Status}.");

            if (s.StartTime != null)
                return (false, "Session đã có thời điểm bắt đầu.");

            s.StartTime = EnsureUtc(startUtc);
            s.Status = "ongoing";
            await _sessionRepo.UpdateAsync(s);

            return (true, "Đã bắt đầu sạc.");
        }

        public async Task<(bool ok, string msg, decimal energyKwh)> StopAsync(int sessionId, DateTime endUtc)
        {
            var s = await _sessionRepo.GetByIdAsync(sessionId);
            if (s == null) return (false, "Không tìm thấy session.", 0m);

            if (!string.Equals(s.Status, "ongoing", StringComparison.OrdinalIgnoreCase))
                return (false, $"Không thể dừng, trạng thái hiện tại: {s.Status}.", 0m);

            if (s.StartTime == null) return (false, "Chưa có thời điểm bắt đầu.", 0m);
            if (s.EndTime != null) return (false, "Session đã dừng trước đó.", 0m);

            var start = EnsureUtc(s.StartTime.Value);
            var end = EnsureUtc(endUtc);
            if (end <= start) return (false, "Thời điểm kết thúc phải > bắt đầu.", 0m);

            var p = s.Point ?? await _pointRepo.GetByIdAsync(s.PointId);

            var portType = p?.PortType?.Trim() ?? "";
            var powerLevel = (decimal)(p?.PowerLevelKw ?? 0); // kWh mục tiêu
            var speedRaw = (decimal)(p?.ChargingSpeedKw ?? 0); // AC: phút để đầy; ngược lại: kW

            // KHỚP client: AC && powerLevel>0 && 5..600 phút => minutes-to-full; ngược lại: kW
            var isMinutesToFull =
                portType.Equals("AC", StringComparison.OrdinalIgnoreCase)
                && powerLevel > 0
                && speedRaw >= 5m && speedRaw <= 600m;

            decimal energyKwh = 0m;
            var elapsed = end - start;

            if (isMinutesToFull)
            {
                var elapsedMinutes = (decimal)elapsed.TotalMinutes;
                if (elapsedMinutes > 0 && speedRaw > 0)
                {
                    var raw = powerLevel * (elapsedMinutes / speedRaw);
                    energyKwh = Math.Min(raw, powerLevel);
                }
            }
            else
            {
                var elapsedHours = (decimal)elapsed.TotalHours;
                if (elapsedHours > 0 && speedRaw > 0)
                {
                    var raw = speedRaw * elapsedHours;
                    energyKwh = (powerLevel > 0) ? Math.Min(raw, powerLevel) : raw;
                }
            }

            if (energyKwh < 0) energyKwh = 0m;
            energyKwh = decimal.Round(energyKwh, 3, MidpointRounding.AwayFromZero);

            s.EndTime = end;                 // UTC
            s.EnergyConsumedKwh = energyKwh;
            s.Status = "success";
            await _sessionRepo.UpdateAsync(s);

            return (true, "Đã dừng sạc và tính năng lượng.", energyKwh);
        }

        private static DateTime EnsureUtc(DateTime dt) => dt.Kind switch
        {
            DateTimeKind.Utc => dt,
            DateTimeKind.Local => dt.ToUniversalTime(),
            DateTimeKind.Unspecified => DateTime.SpecifyKind(dt, DateTimeKind.Utc)
        };
    }
}
