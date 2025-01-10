using System;
using System.Collections.Generic;

namespace Transportation.Infrastructure.Data;

public partial class Warehouse
{
    public int WarehouseId { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public int Capacity { get; set; }

    public bool IsActive { get; set; }

    public TimeOnly ClosingTime { get; set; }

    public TimeOnly OpeningTime { get; set; }

    public int CustomerId { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
