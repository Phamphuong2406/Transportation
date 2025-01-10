using System;
using System.Collections.Generic;

namespace Transportation.Infrastructure.Data;

public partial class ShippingRequest
{
    public int RequestId { get; set; }

    public DateOnly RequestDate { get; set; }

    public string PickupLocation { get; set; } = null!;

    public decimal? PickupLat { get; set; }

    public decimal? PickupLng { get; set; }

    public string DropoffLocation { get; set; } = null!;

    public decimal? DropoffLat { get; set; }

    public decimal? DropoffLng { get; set; }

    public int? ProductTypeId { get; set; }

    public int Weight { get; set; }

    public int ShippingCost { get; set; }

    public int CustomerId { get; set; }

    public string? Note { get; set; }

    public DateOnly? Pickupdate { get; set; }

    public DateOnly? Deliverydate { get; set; }

    public string? Status { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<DispatchAssignment> DispatchAssignments { get; set; } = new List<DispatchAssignment>();

    public virtual ProductType? ProductType { get; set; }
}
