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
    public class AdminChargingSessionRepository : IAdminChargingSessionRepository
    {
        private readonly EvchargingContext _context;

        public AdminChargingSessionRepository(EvchargingContext context)
        {
            _context = context;
        }

        public async Task<int> CountAllAsync()
            => await _context.ChargingSessions.CountAsync();

        public async Task<int> CountByStatusAsync(string status)
        {
            status = NormalizeStatus(status);
            return await _context.ChargingSessions.CountAsync(s => s.Status == status);
        }

        public async Task<List<ChargingSession>> GetAllAsync()
            => await _context.ChargingSessions
                .Include(s => s.Booking)
                    .ThenInclude(b => b.User)
                .Include(s => s.Point)
                    .ThenInclude(p => p.Station)
                .OrderByDescending(s => s.StartTime)
                .ToListAsync();

        private string NormalizeStatus(string? status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return "coming-soon";

            status = status.Trim().ToLower();
            return status switch
            {
                "ongoing" => "ongoing",
                "success" => "success",
                "coming-soon" => "coming-soon",
                _ => "coming-soon"
            };
        }
    }
}
