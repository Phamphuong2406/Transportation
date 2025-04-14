using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transportation.Domain.Entity
{
    public class UserFunction
    {
        [Key]
        public int UserFunctionId { get; set; }
        public int UserId { get; set; }
        public int FunctionId { get; set; }
        public int IsView { get; set; }
        public int IsUpdate { get; set; }
        public int IsCreate { get; set; }
        public int IsDelete { get; set; }
    }
}
