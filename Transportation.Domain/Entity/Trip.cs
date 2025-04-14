using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Transportation.Infrastructure.Data;

public partial class Trip
{
    [Key]
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
