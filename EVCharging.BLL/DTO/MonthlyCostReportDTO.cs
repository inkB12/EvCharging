namespace EVCharging.BLL.DTO
{
    public class MonthlyCostReportDTO
    {
        public required string MonthYear { get; set; }
        public int TotalSessions { get; set; }
        public decimal TotalCost { get; set; }
        public decimal? TotalEnergyKWh { get; set; }
    }
}
