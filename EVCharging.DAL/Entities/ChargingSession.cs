namespace EVCharging.DAL.Entities;

public partial class ChargingSession
{
    public int Id { get; set; }

    public int BookingId { get; set; }

    public int PointId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public decimal? EnergyConsumedKwh { get; set; }

    public string Status { get; set; } = null!;

    public virtual Booking Booking { get; set; } = null!;

    public virtual ChargingPoint Point { get; set; } = null!;
}
