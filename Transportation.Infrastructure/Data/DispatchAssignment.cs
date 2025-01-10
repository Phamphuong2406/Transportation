using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Transportation.Infrastructure.Data;

public partial class DispatchAssignment
{
    public int AssignmentId { get; set; }

    public int RequestId { get; set; }

    public int TruckId { get; set; }

    public int AssignedBy { get; set; }

    public DateOnly AssignedDate { get; set; }

    public DateOnly RequestDate { get; set; }

    public string? PickupLocation { get; set; }

    public decimal? PickupLat { get; set; }

    public decimal? PickupLng { get; set; }

    public string? DropoffLocation { get; set; }

    public decimal? DropoffLat { get; set; }

    public decimal? DropoffLng { get; set; }

    public int? Weight { get; set; }

    public int? ProductTypeId { get; set; }

    public int? ShippingCost { get; set; }

    public decimal? ParkingLat { get; set; }

    public decimal? ParkingLng { get; set; }

    public int? Capacity { get; set; }

    public string? FuelType { get; set; }

    public string? Note { get; set; }

    public string? Status { get; set; }

    public string? DeliveryImage { get; set; }

    public DateOnly? Pickupdate { get; set; }

    public DateOnly? Deliverydate { get; set; }

    [NotMapped]
    [FileExtensions]

    public IFormFile ImageUpload {  get; set; }

    public virtual Dispatcher AssignedByNavigation { get; set; } = null!;

    public virtual ShippingRequest Request { get; set; } = null!;

    public virtual Truck Truck { get; set; } = null!;
}
