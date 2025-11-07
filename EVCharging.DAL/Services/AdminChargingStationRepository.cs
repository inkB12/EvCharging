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
    public class AdminChargingStationRepository : IAdminChargingStationRepository
    {
        private readonly EvchargingContext _context;

        public AdminChargingStationRepository(EvchargingContext context)
        {
            _context = context;
        }

        public async Task<List<ChargingStation>> GetAllAsync()
        {
            return await _context.ChargingStations.ToListAsync();
        }

        public async Task<ChargingStation?> GetByIdAsync(int id)
        {
            return await _context.ChargingStations
                .Include(s => s.ChargingPoints)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(ChargingStation station)
        {
            _context.ChargingStations.Add(station);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChargingStation station)
        {
            _context.ChargingStations.Update(station);
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

        public async Task UpdateStationStatusAsync(int id, string status)
        {
            var station = await _context.ChargingStations.FindAsync(id);
            if (station != null)
            {
                station.Status = status;
                await _context.SaveChangesAsync();
            }
        }
    }
}
