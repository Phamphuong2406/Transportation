using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transportation.ViewModel.Register
{
    public class TruckRegistrationVM
    {
        [Display(Name = "Tên tài xế")]
        [Required(ErrorMessage = "bạn chưa nhập tên tài xế")]
        public int? IdDriver { get; set; }
        [Display(Name = "Tải trọng")]
        [Required(ErrorMessage = "bạn chưa nhập tải trọng")]
        public decimal Capacity { get; set; }
        [Display(Name = "Nhiên liệu")]
        [Required(ErrorMessage = "bạn chưa nhập nhiên liệu")]
        public string? FuelType { get; set; }
        [Display(Name = "Định mức tiêu thụ")]
        [Required(ErrorMessage = "bạn chưa nhập Định mức")]
        public decimal ConsumptionRate { get; set; }
        [Display(Name = "Điểm đỗ")]
        [Required(ErrorMessage = "bạn chưa nhập Điểm đỗ")]
        public string? ParkingLocation { get; set; }
    

    }
}
