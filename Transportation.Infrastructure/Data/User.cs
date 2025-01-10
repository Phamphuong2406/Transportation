using System;
using System.Collections.Generic;

namespace Transportation.Infrastructure.Data;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int RoleId { get; set; }

    public string Email { get; set; } = null!;

    public bool? IsActive { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string RandomKey { get; set; } = null!;

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Dispatcher> Dispatchers { get; set; } = new List<Dispatcher>();

    public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();

    public virtual Role Role { get; set; } = null!;
}
