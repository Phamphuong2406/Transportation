using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class ShippingRequetsDTO
    {
        public int? RequestId { get; set; }

        public DateOnly? RequestDate { get; set; }

        public string? PickupLocation { get; set; }


        public string? DropoffLocation { get; set; }

        public int Weight { get; set; }

        public int? ShippingCost { get; set; }

        public string? Note { get; set; }

        public DateTime? Pickupdate { get; set; }

        public DateTime? Deliverydate { get; set; }

        public string? Status { get; set; }
        public decimal? PickupLat { get; set; }
        public decimal? PickupLng { get; set; }
        public decimal? DropoffLat { get; set; }
        public decimal? DropoffLng { get; set; }
        public int WarehouseId { get; set; }
        public int ProductTypeId { get; set; }
        public string? CustomerName { get; set; }
        public string? ProductNameType{ get; set; }
    }
    /* public class ShippingRequestDto
     {
         public string DropoffLocation { get; set; }
         public int WarehouseId { get; set; }

         public float Weight { get; set; }
         public DateTime Pickupdate { get; set; }
         public DateTime Deliverydate { get; set; }
         public string Note { get; set; }
         public decimal FromLatitude { get; set; }
         public decimal FromLongitude { get; set; }
         public decimal ToLatitude { get; set; }
         public decimal ToLongitude { get; set; }
         public int ProductTypeId { get; set; }
     }*/



}
