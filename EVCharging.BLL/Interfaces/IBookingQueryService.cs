using EVCharging.BLL.DTO;

namespace EVCharging.BLL.Interfaces
{
    public interface IBookingQueryService
    {
        Task<List<BookingListItemDTO>> GetMyBookingsAsync(int userId);
        Task<BookingDetailDTO?> GetDetailAsync(int userId, int bookingId);
    }
}
