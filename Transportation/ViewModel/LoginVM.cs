using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transportation.ViewModel
{
    public class LoginVM
    {
        [Display(Name = "tên đăng nhập")]
        [Required(ErrorMessage = "bạn chưa nhập tên đăng nhập")]

        public string Username { get; set; } = null!;
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "bạn chưa nhập Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
