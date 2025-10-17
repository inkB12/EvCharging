using System;
using System.Collections.Generic;

namespace EVCharging.DAL.Entities;

public partial class Booking
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime BookingTime { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public decimal Price { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<ChargingSession> ChargingSessions { get; set; } = new List<ChargingSession>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual User User { get; set; } = null!;
}
