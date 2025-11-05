using EVCharging.BLL.DTO;

namespace EVCharging.BLL.Interfaces
{
    public interface IStaffBookingQueryService
    {
        Task<List<BookingListItemDTO>> GetByStationAsync(int stationId);
        Task<BookingDetailDTO?> GetDetailAsync(int? stationId, int bookingId);
    }
}
