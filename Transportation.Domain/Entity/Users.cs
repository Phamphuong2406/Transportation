using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transportation.Domain.Entity
{
    public partial class Users
    {
        [Key]
        public int Id { get; set; }

        public string? UserName { get; set; } 

        public string? Password { get; set; }
        public string? FullName {  get; set; } 
        public string? Address {  get; set; } 
        public string? RefreshToken {  get; set; } 
        public DateTime? RefreshTokenExprired { get; set; } 
    }
}
