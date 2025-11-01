using EVCharging.DAL.Entities;
using EVCharging.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EVCharging.DAL.Services
{
    public class ChargingSessionRepository(EvchargingContext context) : IChargingSessionRepository
    {
        private readonly EvchargingContext _context = context;

        public async Task<ChargingSession?> GetByIdAsync(int sessionId)
        {
            return await _context.ChargingSessions
                .Include(s => s.Point)
                .Include(s => s.Booking).ThenInclude(b => b.User).ThenInclude(u => u.ServicePlan)
                .FirstOrDefaultAsync(session => session.Id == sessionId);
        }

        public async Task<List<ChargingSession>> GetAllByUserAndYear(int userId, int year)
        {
            return await _context.ChargingSessions
                .Include(s => s.Point).ThenInclude(p => p.Station)
                .Where(c => c.Booking.User.Id == userId && c.StartTime.Year == year && c.Status.Equals("SUCCESS"))
                .ToListAsync();
        }
    }
}
