using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
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
    }
}
