using System;
using System.Collections.Generic;

namespace DataAccess.Entity;

public partial class RealTimeTracking
{
    public int TrackingId { get; set; }

    public int TruckId { get; set; }

    public DateTime Timestamp { get; set; }

    public decimal CurrentLat { get; set; }

    public decimal CurrentLng { get; set; }

    public virtual Truck Truck { get; set; } = null!;
}
