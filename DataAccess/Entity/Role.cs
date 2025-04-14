using System;
using System.Collections.Generic;

namespace DataAccess.Entity;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;
    public ICollection<UserRole> UserRoles { get; set; }
}
