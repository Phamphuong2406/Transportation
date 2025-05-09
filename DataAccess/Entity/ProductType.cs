﻿using System;
using System.Collections.Generic;

namespace DataAccess.Entity;

public partial class ProductType
{
    public int ProductTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<ShippingRequest> ShippingRequests { get; set; } = new List<ShippingRequest>();
}
