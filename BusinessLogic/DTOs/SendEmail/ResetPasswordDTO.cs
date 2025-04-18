using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.SendEmail
{
    public class ResetPasswordDTO
    {
        [Required(ErrorMessage ="Password is required")]
        public string? Password {  get; set; }
        [Compare("Password",ErrorMessage ="the password anh confirmation password do not match")]
        public string? ConfirmPassword { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
    }
}
