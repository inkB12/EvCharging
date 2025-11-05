using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using EVCharging.DAL.Interfaces;

namespace EVCharging.BLL.Services
{
    public class StaffBookingQueryService : IStaffBookingQueryService
    {
        private readonly IBookingRepository _bookingRepo;

        public StaffBookingQueryService(IBookingRepository bookingRepo)
        {
            _bookingRepo = bookingRepo;
        }

        // List tất cả booking có session gắn với stationId (qua Point -> Station)
        public async Task<List<BookingListItemDTO>> GetByStationAsync(int stationId)
        {
            var list = await _bookingRepo.GetByStationAsync(stationId);
            var result = new List<BookingListItemDTO>(list.Count);

            foreach (var b in list)
            {
                var firstSession = b.ChargingSessions.FirstOrDefault();
                var point = firstSession?.Point;
                var station = point?.Station;

                DateTime? startUtc = b.StartTime.HasValue ? DateTime.SpecifyKind(b.StartTime.Value, DateTimeKind.Utc) : null;
                DateTime? endUtc = b.EndTime.HasValue ? DateTime.SpecifyKind(b.EndTime.Value, DateTimeKind.Utc) : null;

                result.Add(new BookingListItemDTO
                {
                    BookingId = b.Id,
                    StationName = station?.Name ?? "(Chưa gán trạm)",
                    StationLocation = station?.Location,
                    PortType = point?.PortType ?? "",
                    StartTime = startUtc,
                    EndTime = endUtc,
                    Status = b.Status,
                    Price = b.Price
                });
            }
            return result;
        }

        // Detail: chỉ trả nếu booking thuộc stationId
        public async Task<BookingDetailDTO?> GetDetailAsync(int? stationId, int bookingId)
        {
            var b = await _bookingRepo.GetByIdWithGraphAsync(bookingId);
            if (b == null) return null;

            // hợp lệ khi có ít nhất 1 session thuộc stationId
            var anyMatch = b.ChargingSessions.Any(s => s.Point?.StationId == stationId);
            if (!anyMatch) return null;

            var dto = new BookingDetailDTO
            {
                BookingId = b.Id,
                StartTime = b.StartTime.HasValue ? DateTime.SpecifyKind(b.StartTime.Value, DateTimeKind.Utc) : null,
                EndTime = b.EndTime.HasValue ? DateTime.SpecifyKind(b.EndTime.Value, DateTimeKind.Utc) : null,
                Price = b.Price,
                Status = b.Status
            };

            foreach (var s in b.ChargingSessions)
            {
                dto.Sessions.Add(new SessionItemDTO
                {
                    SessionId = s.Id,
                    Status = s.Status,
                    StartTime = s.StartTime.HasValue ? DateTime.SpecifyKind(s.StartTime.Value, DateTimeKind.Utc) : null,
                    EndTime = s.EndTime.HasValue ? DateTime.SpecifyKind(s.EndTime.Value, DateTimeKind.Utc) : null,
                    EnergyConsumedKwh = s.EnergyConsumedKwh,
                    PointId = s.PointId,
                    PortType = s.Point?.PortType ?? "",
                    StationName = s.Point?.Station?.Name ?? "",
                    StationLocation = s.Point?.Station?.Location
                });
            }
            return dto;
        }
    }
}
