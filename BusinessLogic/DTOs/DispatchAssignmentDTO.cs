using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class DispatchAssignmentDTO
    {
        public int? AssignmentId { get; set; }

        public int? RequestId { get; set; }

        public int? AssignedBy { get; set; }

        public DateOnly? AssignedDate { get; set; }

        public DateOnly? RequestDate { get; set; }

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

        public IFormFile? ImageFile { get; set; }

        public DateTime? Pickupdate { get; set; }

        public DateTime? Deliverydate { get; set; }

        public int? TripId { get; set; }
    }
    public class DispatchAssignmentUpdateDTO
    {
        [Required]
        public string? DeliveryImage { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? Status { get; set; }
    }
    public class AssignTripRequestDTO
    {
        public int RequestId { get; set; }
        public int TripId { get; set; }
    }
    public class UpdateStatusRequest
    {
        public int RequestId { get; set; }
    }
    public class OrderStatusDTO
    {
        public string Status { get; set; }
        public int Count { get; set; }

    }
    public class LateDeliveryDataDTO()
    {
        public int total { get; set; }
        public int late { get; set; }
        public int onTime { get; set; }
    }
    public class CompareordersoftruckDTO
    {
        public int? TruckId { get; set; }
        public int? TotalProcessed { get; set; }
        public string? Status { get; set; }
    }
    public class CompareRevenueDTO
    {
        public int Date { get; set; }
        public int? Cost { get; set; }
    }
    public class CargoWeightChartDTO
    {
        public int Date { get; set; }
        public int? TotalWeight { get; set; }
    }
}
