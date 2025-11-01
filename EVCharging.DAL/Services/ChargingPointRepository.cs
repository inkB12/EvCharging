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
    public class ChargingPointRepository : IChargingPointRepository
    {
        private readonly EvchargingContext _context;

        public ChargingPointRepository(EvchargingContext context)
        {
            _context = context;
        }

        public async Task<List<ChargingPoint>> GetPointsByStationAsync(int stationId)
        {
            return await _context.ChargingPoints
                .Where(p => p.StationId == stationId)
                .ToListAsync();
        }

        public async Task<ChargingPoint?> GetByIdAsync(int id)
        {
            return await _context.ChargingPoints.FindAsync(id);
        }

        public async Task<int> CreateAsync(ChargingPoint entity)
        {
            _context.ChargingPoints.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(ChargingPoint entity)
        {
            _context.ChargingPoints.Update(entity);
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

        public async Task<int> CountPointsByStatusAsync(string status)
        {
            return await _context.ChargingPoints.CountAsync(p => p.Status == status);
        }

        public async Task<List<ChargingPoint>> GetPointsByStatusAsync(string status)
        {
            return await _context.ChargingPoints.Where(p => p.Status == status).ToListAsync();
        }
    }
}
