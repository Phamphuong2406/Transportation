using System;
using System.Collections.Generic;

namespace DataAccess.Entity;

public partial class Trip
{
    public int TripId { get; set; }

    public int? ShiftId { get; set; }

    public int? TruckId { get; set; }

    public DateOnly? AssignedDate { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<DispatchAssignment> DispatchAssignments { get; set; } = new List<DispatchAssignment>();

    public virtual Shift? Shift { get; set; }

    public virtual Truck? Truck { get; set; }
}
