using EVCharging.BLL.Interfaces;
using EVCharging.DAL.Interfaces;

namespace EVCharging.BLL.Services
{
    public class BookingCommandService : IBookingCommandService
    {
        private readonly IBookingRepository _bookingRepo;
        public BookingCommandService(IBookingRepository bookingRepo) => _bookingRepo = bookingRepo;

        public async Task<(bool ok, string msg)> CancelAsync(int userId, int bookingId)
        {
            var b = await _bookingRepo.GetByIdAsync(bookingId);
            if (b == null) return (false, "Booking không tồn tại.");

            if (b.UserId != userId) return (false, "Bạn không có quyền hủy booking này.");

            if (!string.Equals(b.Status, "ongoing", StringComparison.OrdinalIgnoreCase))
                return (false, "Chỉ hủy được lịch đang ở trạng thái 'ongoing'.");

            // so sánh trên UTC, vì ta lưu UTC
            var nowUtc = DateTime.UtcNow;
            if (b.StartTime.HasValue && b.StartTime.Value <= nowUtc)
                return (false, "Không thể hủy lịch đã bắt đầu hoặc đã qua.");

            b.Status = "cancelled";
            await _bookingRepo.UpdateAsync(b);

            return (true, "Đã hủy lịch thành công.");
        }
    }
}
