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
    public class AdminChargingStationService : IAdminChargingStationService
    {
        private readonly IAdminChargingStationRepository _repository;

        public AdminChargingStationService(IAdminChargingStationRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AdminChargingStationDTO>> GetAllAsync()
        {
            var stations = await _repository.GetStationsWithPointsAsync();
            return stations.Select(MapToDTO).ToList();
        }

        public async Task<AdminChargingStationDTO?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : MapToDTO(entity);
        }

        public async Task AddAsync(AdminChargingStationDTO dto)
        {
            var entity = MapToEntity(dto);
            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(AdminChargingStationDTO dto)
        {
            var entity = MapToEntity(dto);
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);

        public async Task UpdateStatusAsync(int id, string status) =>
            await _repository.UpdateStationStatusAsync(id, status);

        private AdminChargingStationDTO MapToDTO(ChargingStation e)
        {
            return new AdminChargingStationDTO
            {
                Id = e.Id,
                Name = e.Name,
                Location = e.Location,
                Description = e.Description,
                Status = e.Status,
                Latitude = e.Latitude,
                Longtitude = e.Longtitude,
                ChargingPoints = e.ChargingPoints?.Select(p => new AdminChargingPointDTO
                {
                    Id = p.Id,
                    StationId = p.StationId,
                    PortType = p.PortType,
                    Status = p.Status,
                    PowerLevelKw = (decimal)p.PowerLevelKw,
                    ChargingSpeedKw = (decimal)p.ChargingSpeedKw,
                    Price = p.Price
                }).ToList()
            };
        }

        private ChargingStation MapToEntity(AdminChargingStationDTO d)
        {
            return new ChargingStation
            {
                Id = d.Id,
                Name = d.Name ?? "",
                Location = d.Location ?? "",
                Description = d.Description ?? "",
                Status = d.Status ?? "empty",
                Latitude = d.Latitude,
                Longtitude = d.Longtitude
            };
        }
    }
}
