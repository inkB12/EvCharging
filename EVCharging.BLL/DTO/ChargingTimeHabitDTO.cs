namespace EVCharging.BLL.DTO
{
    public class ChargingTimeHabitDTO
    {
        public string TimeRangeLabel { get; set; } = default!;
        public int HourOfDay { get; set; }
        public int SessionCount { get; set; }
    }
}
