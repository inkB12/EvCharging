namespace EVCharging.BLL.DTO
{
    public class ChargingLocationHabitDTO
    {
        public string StationName { get; set; } = default!;
        public string Location { get; set; } = default!;
        public int SessionCount { get; set; } = default;
    }
}
