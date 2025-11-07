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
    public class AdminChargingPointService : IAdminChargingPointService
    {
        private readonly IAdminChargingPointRepository _repository;

        public AdminChargingPointService(IAdminChargingPointRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AdminChargingPointDTO>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(MapToDTO).ToList();
        }

        public async Task<AdminChargingPointDTO?> GetByIdAsync(int id)
        {
            var e = await _repository.GetByIdAsync(id);
            return e == null ? null : MapToDTO(e);
        }

        public async Task AddAsync(AdminChargingPointDTO dto)
        {
            await _repository.AddAsync(MapToEntity(dto));
        }

        public async Task UpdateAsync(AdminChargingPointDTO dto)
        {
            await _repository.UpdateAsync(MapToEntity(dto));
        }

        public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);

        public async Task<List<AdminChargingPointDTO>> GetByStatusAsync(string status)
        {
            var list = await _repository.GetPointsByStatusAsync(status);
            return list.Select(MapToDTO).ToList();
        }

        public async Task<bool> SetPointStatusAsync(int id, string status)
        {
            return await _repository.SetChargingPointStatusAsync(id, status);
        }

        private AdminChargingPointDTO MapToDTO(ChargingPoint e) => new()
        {
            Id = e.Id,
            StationId = e.StationId,
            PortType = e.PortType,
            Status = e.Status,
            PowerLevelKw = (decimal)e.PowerLevelKw,
            ChargingSpeedKw = (decimal)e.ChargingSpeedKw,
            Price = e.Price
        };

        private ChargingPoint MapToEntity(AdminChargingPointDTO d) => new()
        {
            Id = d.Id,
            StationId = d.StationId,
            PortType = d.PortType ?? "AC",
            Status = d.Status ?? "online",
            PowerLevelKw = (int?)d.PowerLevelKw,
            ChargingSpeedKw = (int?)d.ChargingSpeedKw,
            Price = d.Price
        };
    }
}

