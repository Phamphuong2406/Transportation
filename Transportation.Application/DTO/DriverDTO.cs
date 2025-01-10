using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transportation.Application.DTO
{
    public  class DriverDTO
    {
        [Display(Name = "tên đăng nhập")]
        [Required(ErrorMessage = "bạn chưa nhập tên đăng nhập")]
        [MaxLength(20, ErrorMessage = "Tối đa 20 ký tự")]
        public string Username { get; set; } = null!;

        [Display(Name = "Email")]
        [Required(ErrorMessage = "bạn chưa nhập Email")]
        public string Email { get; set; } = null!;

        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "bạn chưa nhập số điênj thoại")]
        public string? PhoneNumber { get; set; }
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "bạn chưa nhập Mật khẩu")]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; } = null!;
        [Display(Name = "Tên đầy đủ")]
        [Required(ErrorMessage = "bạn chưa nhập tên đầy đủ")]
        public string FullName { get; set; } = null!;
        [Display(Name = "Ngày sinh")]
        [Required(ErrorMessage = "bạn chưa nhập ngày sinh")]
        public DateOnly? DateOfBirth { get; set; }
        [Display(Name = "Số CCCD")]
        [Required(ErrorMessage = "bạn chưa nhập số CCCD")]
        public string? Idcard { get; set; }
        [Display(Name = "Tình Trạng sức khỏe")]
        [Required(ErrorMessage = "bạn chưa nhập tình trạng sức khỏe")]
        public string? HealthStatus { get; set; }

    }
}
