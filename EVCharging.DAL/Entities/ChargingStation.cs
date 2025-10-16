using System;
using System.Collections.Generic;

namespace EVCharging.DAL.Entities;

public partial class ChargingStation
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Location { get; set; }

    public string? Code { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<ChargingPoint> ChargingPoints { get; set; } = new List<ChargingPoint>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
