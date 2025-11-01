using EVCharging.BLL.DTO;

namespace EVCharging.BLL.Interfaces
{
    public interface IReportService
    {
        Task<ReportDto> GetSystemReportAsync(DateTime? start = null, DateTime? end = null);
        Task<List<MonthlyCostReportDTO>> GetMonthlyCostReportByYear(int userId, int year);
    }
}
