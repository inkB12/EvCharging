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
    public class AdminChargingPointRepository : IAdminChargingPointRepository
    {
        private readonly EvchargingContext _context;

        public AdminChargingPointRepository(EvchargingContext context)
        {
            _context = context;
        }

        public async Task<List<ChargingPoint>> GetAllAsync()
        {
            return await _context.ChargingPoints.Include(p => p.Station).ToListAsync();
        }

        public async Task<ChargingPoint?> GetByIdAsync(int id)
        {
            return await _context.ChargingPoints
                .Include(p => p.Station)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(ChargingPoint point)
        {
            _context.ChargingPoints.Add(point);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChargingPoint point)
        {
            _context.ChargingPoints.Update(point);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var point = await _context.ChargingPoints.FindAsync(id);
            if (point != null)
            {
                _context.ChargingPoints.Remove(point);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ChargingPoint>> GetPointsByStatusAsync(string status)
        {
            return await _context.ChargingPoints
                .Where(p => p.Status == status)
                .Include(p => p.Station)
                .ToListAsync();
        }

        public async Task<bool> SetChargingPointStatusAsync(int id, string status)
        {
            var point = await _context.ChargingPoints.FindAsync(id);
            if (point == null) return false;

            point.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
