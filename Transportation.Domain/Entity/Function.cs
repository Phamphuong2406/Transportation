using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transportation.Domain.Entity
{
   public class Function
    {
        [Key]
        public int FunctionId { get; set; }
      public string? FunctionName { get; set; }
      public string? FunctionCode { get; set; }
    }
}
