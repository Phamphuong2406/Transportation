using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transportation.Application.DTO
{
    public class RegisterModel

    {
        [Required(ErrorMessage = "User name is required")]
        public string? Username { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "email is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password  is required")]
        public string? Password { get; set; }
    }
}
