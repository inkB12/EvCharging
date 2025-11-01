namespace EVCharging.BLL.DTO
{
    public class YearlyCostOverviewDTO
    {
        public decimal TotalCostYear { get; set; }
        public decimal TotalEnergyKWhYear { get; set; }
        public int TotalSessionsYear { get; set; }
        public List<MonthlyCostReportDTO> MonthlyData { get; set; } = [];
    }
}
