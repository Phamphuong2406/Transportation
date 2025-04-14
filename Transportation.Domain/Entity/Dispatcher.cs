using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Transportation.Infrastructure.Data;

public partial class Dispatcher
{
    [Key]
    public int DispatcherId { get; set; }

    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Department { get; set; }

    public virtual ICollection<DispatchAssignment> DispatchAssignments { get; set; } = new List<DispatchAssignment>();

    public virtual User User { get; set; } = null!;
}
