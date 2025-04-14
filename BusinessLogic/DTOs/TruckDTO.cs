namespace BusinessLogic.DTOs
{
    public class TruckDTO
    {
        public int? TruckId { get; set; }

        public int? DriverId { get; set; }

        public int? Capacity { get; set; }

        public string? FuelType { get; set; }
        public decimal? ConsumptionRate { get; set; }

        public string? ParkingLocation { get; set; }

        public decimal? ParkingLat { get; set; }

        public decimal? ParkingLng { get; set; }

        public string? LicensePlate { get; set; }
    }
}
