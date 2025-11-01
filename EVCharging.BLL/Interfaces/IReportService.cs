using EVCharging.BLL.DTO;

namespace EVCharging.BLL.Interfaces
{
    public interface IReportService
    {
        Task<ReportDto> GetSystemReportAsync(DateTime? start = null, DateTime? end = null);
        Task<YearlyCostOverviewDTO> GetMonthlyCostReportByYear(int userId, int year);
        Task<List<ChargingLocationHabitDTO>> GetChargingLocationHabits(int userId, int year);
        Task<List<ChargingTimeHabitDTO>> GetChargingTimeHabits(int userId, int year);
    }
}
