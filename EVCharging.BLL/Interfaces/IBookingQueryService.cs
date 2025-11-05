using EVCharging.BLL.DTO;

namespace EVCharging.BLL.Interfaces
{
    public interface IBookingQueryService
    {
        Task<List<BookingListItemDTO>> GetMyBookingsAsync(int userId);
    }
}
