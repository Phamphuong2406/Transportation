using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transportation.Application.DTO
{
    public class WarehouseDto
    {


        public int WarehouseId { get; set; }

        public string Name { get; set; } = null!;

        public string Address { get; set; } = null!;

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public int? Capacity { get; set; }

        public bool IsActive { get; set; }

        public TimeOnly? ClosingTime { get; set; }

        public TimeOnly? OpeningTime { get; set; }

    }
}
