public class ChargeSessionRuntimeDTO
{
    public int SessionId { get; set; }
    public int BookingId { get; set; }
    public int PointId { get; set; }

    public string? StationName { get; set; }
    public string? StationLocation { get; set; }

    public string? PortType { get; set; }
    public int? PowerLevelKW { get; set; }  
    public int? ChargingSpeedKW { get; set; } 

    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string Status { get; set; } = null!;
    public decimal? EnergyConsumedKwh { get; set; }
}
