using EVCharging.BLL.DTO;
using EVCharging.BLL.Interfaces;
using EVCharging.DAL.Entities;
using EVCharging.DAL.Interfaces;

namespace EVCharging.BLL.Services
{
    public class ChargingStationService : IChargingStationService
    {
        private readonly IChargingStationRepository _repo;

        public ChargingStationService(IChargingStationRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<ChargingStationDto>> GetAllAsync()
        {
            var list = await _repo.GetAllStationsAsync();
            return list.Select(s => new ChargingStationDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Location = s.Location,
                Station = s.Station,
                Status = s.Status,
                Longtitude = s.Longtitude,
                Latitude = s.Latitude
            }).ToList();
        }

        public async Task<ChargingStationDto?> GetByIdAsync(int id)
        {
            var s = await _repo.GetByIdAsync(id);
            if (s == null) return null;
            return new ChargingStationDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Location = s.Location,
                Station = s.Station,
                Status = s.Status,
                Latitude = s.Latitude,
                Longtitude = s.Longtitude
            };
        }

        public async Task<int> CreateAsync(ChargingStationDto dto)
        {
            var entity = new ChargingStation
            {
                Name = dto.Name,
                Description = dto.Description,
                Location = dto.Location,
                Station = dto.Station,
                Status = dto.Status,
                Latitude = dto.Latitude,
                Longtitude = dto.Longtitude
            };
            return await _repo.CreateAsync(entity);
        }

        public async Task UpdateAsync(ChargingStationDto dto)
        {
            var entity = new ChargingStation
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Location = dto.Location,
                Station = dto.Station,
                Status = dto.Status,
                Latitude = dto.Latitude,
                Longtitude = dto.Longtitude
            };
            await _repo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }

        public async Task<List<ChargingStationDto>> GetStationsWithPointsAsync()
        {
            var list = await _repo.GetStationsWithPointsAsync();
            return list.Select(s => new ChargingStationDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Location = s.Location,
                Station = s.Station,
                Status = s.Status,
                Longtitude = s.Longtitude,
                Latitude = s.Latitude
            }).ToList();
        }

        public async Task<(int online, int offline)> GetStationStatusSummaryAsync()
        {
            var online = await _repo.CountOnlineStationsAsync();
            var offline = await _repo.CountOfflineStationsAsync();
            return (online, offline);
        }
    }
}
