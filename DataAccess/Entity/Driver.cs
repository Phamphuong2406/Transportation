using System;
using System.Collections.Generic;

namespace DataAccess.Entity;

public partial class Driver
{
    public int DriverId { get; set; }

    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public DateOnly? DateOfBirth { get; set; }

    public string? Idcard { get; set; }

    public string? HealthStatus { get; set; }

    public virtual ICollection<Truck> Trucks { get; set; } = new List<Truck>();

    public virtual Users User { get; set; } = null!;
}
