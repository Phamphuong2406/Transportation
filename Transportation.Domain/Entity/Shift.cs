using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Transportation.Infrastructure.Data;

public partial class Shift
{
    [Key]
    public int ShiftId { get; set; }

    public string? ShiftName { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
