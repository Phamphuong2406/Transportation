using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.Account
{
    public class ForgotPasswordDTO
    {
        [EmailAddress]
        public string? Email {  get; set; }
        public string? ClientUri {  get; set; }
    }
}
