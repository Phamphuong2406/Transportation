using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entity;

public partial class Users
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string? PasswordHash { get; set; } 

    public string Email { get; set; } = null!;

    public bool? IsActive { get; set; }

    public string? PhoneNumber { get; set; } 
    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExprired { get; set; }
    public string? IdToken { get; set; }
    public string? GoogleUserId { get; set; } 




    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Dispatcher> Dispatchers { get; set; } = new List<Dispatcher>();

    public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();
    public ICollection<UserRole> UserRoles { get; set; }
}
