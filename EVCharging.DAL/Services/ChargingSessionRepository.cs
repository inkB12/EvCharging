using EVCharging.DAL.Entities;
using EVCharging.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EVCharging.DAL.Services
{
    public class ChargingSessionRepository : IChargingSessionRepository
    {
        private readonly EvchargingContext _context;
        public ChargingSessionRepository(EvchargingContext context) => _context = context;

        public async Task<ChargingSession?> GetByIdAsync(int sessionId)
        {
            return await _context.ChargingSessions
                .AsNoTracking()
                .Include(s => s.Point)
                .Include(s => s.Booking)
                    .ThenInclude(b => b.User)
                        .ThenInclude(u => u.ServicePlan)
                .FirstOrDefaultAsync(session => session.Id == sessionId);
        }

        public async Task<List<ChargingSession>> GetAllByUserAndYear(int userId, int year)
        {
            var start = new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var end = start.AddYears(1);

            return await _context.ChargingSessions
                .AsNoTracking()
                .Include(s => s.Point)
                    .ThenInclude(p => p.Station)
                .Include(s => s.Booking)
                .Where(s =>
                    s.Booking.UserId == userId &&
                    s.StartTime >= start &&
                    s.StartTime < end &&
                    s.Status == "success" 
                )
                .ToListAsync();
        }

        public async Task<List<int>> GetBusyPointIdsAsync(List<int> pointIds, DateTime start, DateTime end)
        {
            return await _context.ChargingSessions
                .AsNoTracking()
                .Include(s => s.Booking)
                .Where(s =>
                    pointIds.Contains(s.PointId) &&
                    s.Booking.StartTime != null &&
                    s.Booking.EndTime != null &&

                    !(
                        s.Booking.Status != null &&
                        s.Booking.Status.Trim().ToLower() == "cancelled"
                    ) &&

                    s.Booking.StartTime < end &&
                    s.Booking.EndTime > start
                )
                .Select(s => s.PointId)
                .Distinct()
                .ToListAsync();
        }

        public async Task<bool> HasConflictAsync(int pointId, DateTime start, DateTime end)
        {
            return await _context.ChargingSessions
                .Include(s => s.Booking)
                .AnyAsync(s =>
                    s.PointId == pointId &&
                    s.Booking.StartTime != null &&
                    s.Booking.EndTime != null &&

                    !(
                        s.Booking.Status != null &&
                        s.Booking.Status.Trim().ToLower() == "cancelled"
                    ) &&

                    s.Booking.StartTime < end &&
                    s.Booking.EndTime > start
                );
        }

        public async Task<int> CreateAsync(ChargingSession entity)
        {
            _context.ChargingSessions.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }
    }
}
