using EVCharging.DAL.Entities;

namespace EVCharging.DAL.Interfaces
{
    public interface IFaultReportRepository
    {
        Task<IEnumerable<FaultReport>> GetAllFaultReportAsync();
        Task<FaultReport?> GetByIdFaultReportAsync(int id);
        Task<FaultReport?> CreateFaultReportAsync(FaultReport faultReport);
        Task UpdateFaultReportAsync(FaultReport faultReport);
        Task DeleteFaultReportAsync(int id);
    }
}
