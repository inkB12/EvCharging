using EVCharging.BLL.DTO;

namespace EVCharging.BLL.Interfaces
{
    public interface IFaultReportService
    {
        Task<IEnumerable<FaultReportDto?>> GetAllFaultReportAsync();
        Task<FaultReportDto?> GetFaultReportByIdAsync(int id);
        Task CreateFaultReportAsync(FaultReportDto faultReportDto);
        Task DeleteFaultReportAsync(int id);
        Task UpdateFaultReportAsync(FaultReportDto faultReportDto);


    }
}
