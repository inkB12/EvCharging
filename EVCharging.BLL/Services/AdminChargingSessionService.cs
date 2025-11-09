using EVCharging.BLL.AdminDTOs;
using EVCharging.BLL.Interfaces;
using EVCharging.DAL.Entities;
using EVCharging.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.BLL.Services
{
    public class AdminChargingSessionService : IAdminChargingSessionService
    {

        private readonly IAdminChargingSessionRepository _repository;

        public AdminChargingSessionService(IAdminChargingSessionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AdminChargingSessionDTO>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            return list.Select(MapToDTO).ToList();
        }

        public async Task<int> CountAllAsync() => await _repository.CountAllAsync();

        public async Task<int> CountByStatusAsync(string status)
            => await _repository.CountByStatusAsync(status);

        // ===== Helper mapping =====
        private AdminChargingSessionDTO MapToDTO(ChargingSession e)
        {
            return new AdminChargingSessionDTO
            {
                Id = e.Id,
                BookingId = e.BookingId,
                PointId = e.PointId,
                StartTime = e.StartTime,
                EndTime = e.EndTime,
                EnergyConsumedKwh = e.EnergyConsumedKwh,
                Status = e.Status,
                PointPortType = e.Point?.PortType,
                StationName = e.Point?.Station?.Name,
                UserEmail = e.Booking?.User?.Email
            };
        }
    }
}
