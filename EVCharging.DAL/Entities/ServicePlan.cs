using System;
using System.Collections.Generic;

namespace EVCharging.DAL.Entities;

public partial class ServicePlan
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public byte Status { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
