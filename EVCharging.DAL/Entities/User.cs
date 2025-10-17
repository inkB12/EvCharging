using System;
using System.Collections.Generic;

namespace EVCharging.DAL.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? FullName { get; set; }

    public string Password { get; set; } = null!;

    public string? Role { get; set; }

    public string? Vehicle { get; set; }

    public bool IsDeleted { get; set; }

    public int? ServicePlanId { get; set; }

    public int? HomeStationId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<FaultReport> FaultReports { get; set; } = new List<FaultReport>();

    public virtual ChargingStation? HomeStation { get; set; }

    public virtual ServicePlan? ServicePlan { get; set; }
}
