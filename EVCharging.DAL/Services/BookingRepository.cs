using EVCharging.DAL.Entities;
using EVCharging.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EVCharging.DAL.Services
{
    public class BookingRepository : IBookingRepository
    {
        private readonly EvchargingContext _db;
        public BookingRepository(EvchargingContext db) => _db = db;

        public async Task<int> CreateAsync(Booking entity)
        {
            _db.Bookings.Add(entity);
            await _db.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<List<Booking>> GetByUserAsync(int userId)
        {
            return await _db.Bookings
                .AsNoTracking()
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.StartTime)
                .Include(b => b.Transactions)
                .Include(b => b.ChargingSessions)
                    .ThenInclude(s => s.Point)
                        .ThenInclude(p => p.Station)
                .ToListAsync();
        }

        public Task<Booking?> GetByIdAsync(int id)
            => _db.Bookings.FirstOrDefaultAsync(b => b.Id == id);

        public async Task UpdateAsync(Booking entity)
        {
            _db.Bookings.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<Booking?> GetByIdWithGraphAsync(int id)
        {
            return await _db.Bookings
                .Include(b => b.Transactions)
                .Include(b => b.ChargingSessions)
                    .ThenInclude(s => s.Point)
                        .ThenInclude(p => p.Station)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<Booking>> GetByStationAsync(int stationId)
        {
            return await _db.Bookings
                .AsNoTracking()
                .Include(b => b.ChargingSessions)
                    .ThenInclude(s => s.Point)
                        .ThenInclude(p => p.Station)
                .Where(b => b.ChargingSessions.Any(s => s.Point.StationId == stationId))
                .OrderByDescending(b => b.StartTime)
                .ToListAsync();
        }
    }
}
