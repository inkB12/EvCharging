using EVCharging.BLL.AdminDTOs;

namespace EVCharging.BLL.Interfaces
{
    public interface IAdminChargingStationService
    {
        Task<List<AdminChargingStationDTO>> GetAllAsync();
        Task<AdminChargingStationDTO?> GetByIdAsync(int id);
        Task<AdminChargingStationDTO> AddAsync(AdminChargingStationDTO dto);
        Task UpdateAsync(AdminChargingStationDTO dto);
        Task DeleteAsync(int id);
        Task UpdateStatusAsync(int id, string status);
    }
}
