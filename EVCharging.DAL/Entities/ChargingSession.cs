using System;
using System.Collections.Generic;

namespace EVCharging.DAL.Entities;

public partial class ChargingSession
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int PointId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public decimal? EnergyConsumedKwh { get; set; }

    public string Status { get; set; } = null!;

    public virtual ChargingPoint Point { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual User User { get; set; } = null!;
}
