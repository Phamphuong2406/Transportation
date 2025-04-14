using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class TripDTO
    {
        public int? TripId { get; set; }

        public int? ShiftId { get; set; }

        public int? TruckId { get; set; }

        public DateOnly? AssignedDate { get; set; }

        public TimeOnly? StartTime { get; set; }

        public TimeOnly? EndTime { get; set; }

        public string? Status { get; set; }
        public string? ShiftName { get; set; }

    }
    public class GetTripId
    {
        public int TripId { get; set; }
    }
}
