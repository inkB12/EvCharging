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
            station.Status = NormalizeStatus(station.Status);
            _context.ChargingStations.Add(station);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChargingStation station)
        {
            station.Status = NormalizeStatus(station.Status);
            _context.ChargingStations.Update(station);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var station = await _context.ChargingStations.FindAsync(id);
            if (station != null)
            {
                // ❗ Xóa mềm: chỉ đặt lại trạng thái về "empty" (trạm ngừng hoạt động)
                station.Status = "empty";
                _context.ChargingStations.Update(station);
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
                station.Status = NormalizeStatus(status);
                _context.ChargingStations.Update(station);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Chuẩn hóa giá trị Status để tương thích với constraint trong DB.
        /// </summary>
        private string NormalizeStatus(string? status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return "empty";

            status = status.Trim().ToLower();

            return status switch
            {
                "inuse" => "inuse",
                "empty" => "empty",
                // ánh xạ hợp lý để tránh lỗi CHECK constraint
                "online" => "inuse",
                "offline" => "empty",
                "maintenance" => "empty",
                _ => "empty"
            };
        }

    }
}
