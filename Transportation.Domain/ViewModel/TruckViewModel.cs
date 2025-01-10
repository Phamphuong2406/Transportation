using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transportation.Domain.ViewModel
{
    public class TruckViewModel
    {
        public int TruckId { get; set; }
        public string DriverName { get; set; }
        public int Capacity { get; set; }
        public string FuelType { get; set; }
        public string ParkingLocation { get; set; }
    }
}
