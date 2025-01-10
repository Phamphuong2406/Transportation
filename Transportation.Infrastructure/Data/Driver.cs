using System;
using System.Collections.Generic;

namespace Transportation.Infrastructure.Data;

public partial class Driver
{
    public int DriverId { get; set; }

    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public DateOnly? DateOfBirth { get; set; }

    public string? Idcard { get; set; }

    public string? HealthStatus { get; set; }

    public virtual ICollection<Truck> Trucks { get; set; } = new List<Truck>();

    public virtual User User { get; set; } = null!;
}
