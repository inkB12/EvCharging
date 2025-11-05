using EVCharging.DAL.Entities;

namespace EVCharging.DAL.Interfaces
{
    public interface IBookingRepository
    {
        Task<int> CreateAsync(Booking entity);
        Task<List<Booking>> GetByUserAsync(int userId);
        Task<Booking?> GetByIdAsync(int id);
        Task UpdateAsync(Booking entity);
        Task<Booking?> GetByIdWithGraphAsync(int id);
        Task<List<Booking>> GetByStationAsync(int stationId);
    }
}
