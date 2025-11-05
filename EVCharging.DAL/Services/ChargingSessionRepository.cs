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
            // SỬA LẠI: 
            // 1. Xóa .AsNoTracking()
            // 2. Thêm .Include(s => s.Point)
            return await _context.ChargingSessions
                // .AsNoTracking() // <-- XÓA DÒNG NÀY (để có thể Update)
                .Include(s => s.Point) // <-- THÊM DÒNG NÀY (để lấy giá tiền khi Stop)
                .Include(s => s.Booking)
                    .ThenInclude(b => b.User)
                        .ThenInclude(u => u.ServicePlan)
                .FirstOrDefaultAsync(session => session.Id == sessionId);
        }

        public async Task<List<ChargingSession>> GetAllByUserAndYear(int userId, int year)
        {
            // ... (Code của bạn, giữ nguyên) ...
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
            // ... (Code của bạn, giữ nguyên) ...
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
            // ... (Code của bạn, giữ nguyên) ...
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
            // ... (Code của bạn, giữ nguyên) ...
            _context.ChargingSessions.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        // --- THÊM 2 HÀM CÒN THIẾU CHO STAFF ---

        // THÊM: Hàm UpdateAsync cho nút Start/Stop
        public async Task UpdateAsync(ChargingSession entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // THÊM: Hàm GetByStationIdAsync cho Staff xem list
        public async Task<List<ChargingSession>> GetByStationIdAsync(int stationId)
        {
            return await _context.ChargingSessions
                .AsNoTracking() // Dùng AsNoTracking() ở đây vì đây là list Read-Only
                .Include(s => s.Booking)
                    .ThenInclude(b => b.User) // Lấy tên User
                .Include(s => s.Point) // Lấy thông tin Point (PortType, Id)
                .Where(s => s.Point.StationId == stationId) // Lọc theo StationId
                .ToListAsync();
        }
    }
}