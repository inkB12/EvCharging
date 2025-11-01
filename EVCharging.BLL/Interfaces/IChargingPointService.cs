using EVCharging.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCharging.BLL.Interfaces
{
    public interface IChargingPointService
    {
        Task<List<ChargingPointDto>> GetByStationAsync(int stationId);
        Task<ChargingPointDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(ChargingPointDto dto);
        Task UpdateAsync(ChargingPointDto dto);
        Task DeleteAsync(int id);
        Task<List<ChargingPointDto>> GetByStatusAsync(string status);
    }
}
