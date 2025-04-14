using System;
using System.Collections.Generic;

namespace DataAccess.Entity;

public partial class Dispatcher
{
    public int DispatcherId { get; set; }

    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Department { get; set; }

    public virtual ICollection<DispatchAssignment> DispatchAssignments { get; set; } = new List<DispatchAssignment>();

    public virtual Users User { get; set; } = null!;
}
