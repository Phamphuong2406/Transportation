using System;
using System.Collections.Generic;

namespace DataAccess.Entity;

public partial class Customer
{
    public int CustomerId { get; set; }

    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Address { get; set; }

    public virtual ICollection<ShippingRequest> ShippingRequests { get; set; } = new List<ShippingRequest>();

    public virtual Users User { get; set; } = null!;

    public virtual ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
}
