using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transportation.Domain.Entity
{
    public class UserSession
    {
        [Key]
        public int SessionId { get; set; }
        public int? UserId { get; set; }
        public string? Token { get; set; }
        public string? DeviceId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? IP {  get; set; }

    }
}
