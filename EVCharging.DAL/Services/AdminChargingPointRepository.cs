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
            return await _context.ChargingPoints
                .Include(p => p.Station)
                .ToListAsync();
        }

        public async Task<ChargingPoint?> GetByIdAsync(int id)
        {
            return await _context.ChargingPoints
                .Include(p => p.Station)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(ChargingPoint point)
        {
            point.PortType = NormalizePortType(point.PortType);
            point.Status = NormalizeStatus(point.Status);

            _context.ChargingPoints.Add(point);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChargingPoint point)
        {
            var existing = await _context.ChargingPoints
                .FirstOrDefaultAsync(p => p.Id == point.Id);

            if (existing == null)
                return;

            existing.StationId = point.StationId;
            existing.PowerLevelKw = point.PowerLevelKw;
            existing.PortType = NormalizePortType(point.PortType);
            existing.ChargingSpeedKw = point.ChargingSpeedKw;
            existing.Price = point.Price;
            existing.Status = NormalizeStatus(point.Status);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var point = await _context.ChargingPoints
                .FirstOrDefaultAsync(p => p.Id == id);

            if (point != null)
            {
                // ❗ XÓA MỀM: chỉ đặt trạng thái OFFLINE
                point.Status = "offline";
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ChargingPoint>> GetPointsByStatusAsync(string status)
        {
            status = NormalizeStatus(status);

            return await _context.ChargingPoints
                .Where(p => p.Status == status)
                .Include(p => p.Station)
                .ToListAsync();
        }

        public async Task<bool> SetChargingPointStatusAsync(int id, string status)
        {
            var point = await _context.ChargingPoints
                .FirstOrDefaultAsync(p => p.Id == id);

            if (point == null)
                return false;

            point.Status = NormalizeStatus(status);
            await _context.SaveChangesAsync();
            return true;
        }

        private string NormalizeStatus(string? status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return "online"; // default của DB

            status = status.Trim().ToLower();
            return status == "offline" ? "offline" : "online"; // chỉ cho 'online' hoặc 'offline'
        }

        private string NormalizePortType(string? portType)
        {
            if (string.IsNullOrWhiteSpace(portType))
                return "AC"; // default

            var val = portType.Trim().ToUpper();

            // Map về một trong 3 giá trị hợp lệ
            return val switch
            {
                "CCS" => "CCS",
                "CHADEMO" => "CHAdeMO",
                "AC" => "AC",
                _ => "AC"
            };
        }
    }
}
