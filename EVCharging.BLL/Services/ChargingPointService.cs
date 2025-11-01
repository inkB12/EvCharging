using EVCharging.BLL.DTO;
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
    public class ChargingPointService : IChargingPointService
    {
        private readonly IChargingPointRepository _repo;

        public ChargingPointService(IChargingPointRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<ChargingPointDto>> GetByStationAsync(int stationId)
        {
            var list = await _repo.GetPointsByStationAsync(stationId);
            return list.Select(p => new ChargingPointDto
            {
                Id = p.Id,
                StationId = p.StationId,
                PowerLevelKw = p.PowerLevelKw,
                PortType = p.PortType,
                ChargingSpeedKw = p.ChargingSpeedKw,
                Price = p.Price,
                Status = p.Status
            }).ToList();
        }

        public async Task<ChargingPointDto?> GetByIdAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return null;
            return new ChargingPointDto
            {
                Id = p.Id,
                StationId = p.StationId,
                PowerLevelKw = p.PowerLevelKw,
                PortType = p.PortType,
                ChargingSpeedKw = p.ChargingSpeedKw,
                Price = p.Price,
                Status = p.Status
            };
        }

        public async Task<int> CreateAsync(ChargingPointDto dto)
        {
            var entity = new ChargingPoint
            {
                StationId = dto.StationId,
                PowerLevelKw = dto.PowerLevelKw,
                PortType = dto.PortType,
                ChargingSpeedKw = dto.ChargingSpeedKw,
                Price = dto.Price,
                Status = dto.Status
            };
            return await _repo.CreateAsync(entity);
        }

        public async Task UpdateAsync(ChargingPointDto dto)
        {
            var entity = new ChargingPoint
            {
                Id = dto.Id,
                StationId = dto.StationId,
                PowerLevelKw = dto.PowerLevelKw,
                PortType = dto.PortType,
                ChargingSpeedKw = dto.ChargingSpeedKw,
                Price = dto.Price,
                Status = dto.Status
            };
            await _repo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }

        public async Task<List<ChargingPointDto>> GetByStatusAsync(string status)
        {
            var list = await _repo.GetPointsByStatusAsync(status);
            return list.Select(p => new ChargingPointDto
            {
                Id = p.Id,
                StationId = p.StationId,
                Status = p.Status,
                PortType = p.PortType,
                Price = p.Price
            }).ToList();
        }
    }
}
