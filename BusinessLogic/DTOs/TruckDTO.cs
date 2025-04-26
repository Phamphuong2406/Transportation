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
    public class TruckOrderStatisticDTO
    {
        public int? TruckId { get; set; }
        public int TotalOrders { get; set; }
    }
    public class DriverPerformanceDTO
    {
        public string DriverName { get; set; }
        public int TotalTrips { get; set; }
        public int TotalOrders { get; set; }
        public double OnTimeRate { get; set; }
    }
    public class TruckLoadDistributionDTO
    {
        public int? TruckId { get; set; }
        public int? UsedLoad { get; set; }
        public int MaxLoad { get; set; }
    }
}
