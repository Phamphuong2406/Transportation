using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Transportation.Infrastructure.Data;

public partial class Truck
{
    [Key]
    public int TruckId { get; set; }

    public int? DriverId { get; set; }

    public int Capacity { get; set; }

    public string? FuelType { get; set; }

    public decimal ConsumptionRate { get; set; }

    public string? ParkingLocation { get; set; }

    public decimal? ParkingLat { get; set; }

    public decimal? ParkingLng { get; set; }

    public string? LicensePlate { get; set; }

    public virtual Driver? Driver { get; set; }

    public virtual ICollection<RealTimeTracking> RealTimeTrackings { get; set; } = new List<RealTimeTracking>();

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
