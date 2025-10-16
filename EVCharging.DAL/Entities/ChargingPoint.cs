using System;
using System.Collections.Generic;

namespace EVCharging.DAL.Entities;

public partial class ChargingPoint
{
    public int Id { get; set; }

    public int StationId { get; set; }

    public int? PowerLevelKw { get; set; }

    public string PortType { get; set; } = null!;

    public int? ChargingSpeedKw { get; set; }

    public decimal Price { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<ChargingSession> ChargingSessions { get; set; } = new List<ChargingSession>();

    public virtual ChargingStation Station { get; set; } = null!;
}
