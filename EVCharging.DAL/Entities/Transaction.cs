using System;
using System.Collections.Generic;

namespace EVCharging.DAL.Entities;

public partial class Transaction
{
    public int Id { get; set; }

    public int SessionId { get; set; }

    public DateTime Datetime { get; set; }

    public decimal Total { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual ChargingSession Session { get; set; } = null!;
}
