using EVCharging.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.BLL.Interfaces
{
    public interface IChargingStationService
    {
        Task<List<ChargingStationDto>> GetAllAsync();
        Task<ChargingStationDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(ChargingStationDto dto);
        Task UpdateAsync(ChargingStationDto dto);
        Task DeleteAsync(int id);
        Task<List<ChargingStationDto>> GetStationsWithPointsAsync();
        Task<(int online, int offline)> GetStationStatusSummaryAsync();
    }
}
