using EVCharging.DAL.Entities;
using EVCharging.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.DAL.Services
{
    public class ReportRepository : IReportRepository
    {
        private readonly EvchargingContext _context;

        public ReportRepository(EvchargingContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetTotalRevenueAsync(DateTime? start = null, DateTime? end = null)
        {
            var query = _context.Transactions.AsQueryable();
            if (start.HasValue) query = query.Where(t => t.Datetime >= start.Value);
            if (end.HasValue) query = query.Where(t => t.Datetime <= end.Value);

            return await query.SumAsync(t => (decimal?)t.Total ?? 0);
        }

        public async Task<int> CountTotalSessionsAsync(DateTime? start = null, DateTime? end = null)
        {
            var query = _context.ChargingSessions.AsQueryable();
            if (start.HasValue) query = query.Where(s => s.StartTime >= start.Value);
            if (end.HasValue) query = query.Where(s => s.EndTime <= end.Value);

            return await query.CountAsync();
        }

        public async Task<Dictionary<string, int>> GetUsageByStationAsync(DateTime? start = null, DateTime? end = null)
        {
            var query = _context.ChargingSessions
                .Include(s => s.Point)
                .ThenInclude(p => p.Station)
                .AsQueryable();

            if (start.HasValue) query = query.Where(s => s.StartTime >= start.Value);
            if (end.HasValue) query = query.Where(s => s.EndTime <= end.Value);

            return await query
                .GroupBy(s => s.Point.Station.Name)
                .Select(g => new { Station = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Station, x => x.Count);
        }

        public async Task<List<Transaction>> GetTransactionsAsync(DateTime? start = null, DateTime? end = null)
        {
            var query = _context.Transactions.Include(t => t.Booking).AsQueryable();
            if (start.HasValue) query = query.Where(t => t.Datetime >= start.Value);
            if (end.HasValue) query = query.Where(t => t.Datetime <= end.Value);
            return await query.ToListAsync();
        }
    }
}
