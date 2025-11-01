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
    public class ChargingStationRepository : IChargingStationRepository
    {
        private readonly EvchargingContext _context;

        public ChargingStationRepository(EvchargingContext context)
        {
            _context = context;
        }

        public async Task<List<ChargingStation>> GetAllStationsAsync()
        {
            return await _context.ChargingStations
                .Where(s => !string.IsNullOrEmpty(s.Status))
                .ToListAsync();
        }

        public async Task<ChargingStation?> GetByIdAsync(int id)
        {
            return await _context.ChargingStations
                .Include(s => s.ChargingPoints)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<int> CreateAsync(ChargingStation entity)
        {
            _context.ChargingStations.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(ChargingStation entity)
        {
            _context.ChargingStations.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var station = await _context.ChargingStations.FindAsync(id);
            if (station != null)
            {
                _context.ChargingStations.Remove(station);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ChargingStation>> GetStationsWithPointsAsync()
        {
            return await _context.ChargingStations
                .Include(s => s.ChargingPoints)
                .ToListAsync();
        }

        public async Task<int> CountOnlineStationsAsync()
        {
            return await _context.ChargingStations.CountAsync(s => s.Status == "Online");
        }

        public async Task<int> CountOfflineStationsAsync()
        {
            return await _context.ChargingStations.CountAsync(s => s.Status == "Offline");
        }
    }
}
