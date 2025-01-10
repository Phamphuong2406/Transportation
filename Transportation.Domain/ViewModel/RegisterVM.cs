using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transportation.Domain.ViewModel
{
    public class RegisterVM
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

        // Thông tin khách hàng cho bảng Customers
        [Display(Name = "Tên đầy đủ")]
        [Required(ErrorMessage = "bạn chưa nhập Tên đầy đủ")]

        public string FullName { get; set; }

        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "bạn chưa nhập địa chỉ")]
        public string Address { get; set; }
    }
}
