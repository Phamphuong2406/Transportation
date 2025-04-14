using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entity;

public partial class UserRole
{
    
    public int UserId { get; set; }


    public int RoleId { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual Users User { get; set; } = null!;
}
