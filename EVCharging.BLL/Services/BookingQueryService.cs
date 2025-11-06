using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using EVCharging.DAL.Entities;
using EVCharging.DAL.Interfaces;

namespace EVCharging.BLL.Services
{
    public class BookingQueryService : IBookingQueryService
    {
        private readonly IBookingRepository _bookingRepo;

        public BookingQueryService(IBookingRepository bookingRepo)
        {
            _bookingRepo = bookingRepo;
        }

        public async Task<List<BookingListItemDTO>> GetMyBookingsAsync(int userId)
        {
            var list = await _bookingRepo.GetByUserAsync(userId);

            var result = new List<BookingListItemDTO>(list.Count);
            foreach (var b in list)
            {
                var firstSession = b.ChargingSessions.FirstOrDefault();
                var point = firstSession?.Point;
                var station = point?.Station;

                // Gắn Kind=Utc để UI có thể ToLocalTime() chính xác
                DateTime? startUtc = b.StartTime.HasValue
                    ? DateTime.SpecifyKind(b.StartTime.Value, DateTimeKind.Utc)
                    : (DateTime?)null;

                DateTime? endUtc = b.EndTime.HasValue
                    ? DateTime.SpecifyKind(b.EndTime.Value, DateTimeKind.Utc)
                    : (DateTime?)null;

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

        public async Task<BookingDetailDTO?> GetDetailAsync(int userId, int bookingId)
        {
            var b = await _bookingRepo.GetByIdWithGraphAsync(bookingId);
            if (b == null || b.UserId != userId) return null;

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

            foreach (var t in b.Transactions)
            {
                var tDto = MapToDTO(t);
                if (tDto != null) dto.Transactions.Add(tDto);
            }

            return dto;
        }

        private TransactionDTO? MapToDTO(Transaction entity)
        {
            if (entity == null) return null;

            return new TransactionDTO()
            {
                BookingId = entity.BookingId,
                Datetime = entity.Datetime,
                PaymentMethod = entity.PaymentMethod,
                Status = entity.Status,
                Id = entity.Id,
                Total = entity.Total,
            };
        }
    }
}
