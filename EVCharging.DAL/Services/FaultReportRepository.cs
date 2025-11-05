using EVCharging.DAL.Entities;
using EVCharging.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EVCharging.DAL.Services
{
    public class FaultReportRepository : IFaultReportRepository
    {
        private readonly EvchargingContext _context;
        public FaultReportRepository(EvchargingContext context)
        {
            _context = context;
        }

        public async Task<FaultReport?> CreateFaultReportAsync(FaultReport faultReport)
        {
            _context.FaultReports.Add(faultReport);
            await _context.SaveChangesAsync();
            return faultReport;
        }

        public async Task DeleteFaultReportAsync(int id)
        {
            var faltReport = await GetByIdFaultReportAsync(id);
            if (faltReport != null)
            {
                _context.FaultReports.Remove(faltReport);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<FaultReport>> GetAllFaultReportAsync()
        {
            var all = await _context.FaultReports
                .Include(fr => fr.Point) // Dùng 'Point' (theo entity)
                .Include(fr => fr.User)  // Dùng 'User' (theo entity)
                .AsNoTracking()
                .ToListAsync();
            return all;
        }

        public async Task<FaultReport?> GetByIdFaultReportAsync(int id)
        {
            var result = await _context.FaultReports.FindAsync(id);
            return result;
        }

        public async Task UpdateFaultReportAsync(FaultReport faultReport)
        {
            _context.Entry(faultReport).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }

}
