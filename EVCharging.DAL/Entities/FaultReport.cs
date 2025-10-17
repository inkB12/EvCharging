using System;
using System.Collections.Generic;

namespace EVCharging.DAL.Entities;

public partial class FaultReport
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int PointId { get; set; }

    public DateTime ReportTime { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public byte Severity { get; set; }

    public string Status { get; set; } = null!;

    public virtual ChargingPoint Point { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
