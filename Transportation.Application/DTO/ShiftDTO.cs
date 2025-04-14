using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transportation.Application.DTO
{
    public class ShiftDTO
    {
        public int ShiftId { get; set; }  // Nếu = 0 thì là tạo mới, ngược lại là cập nhật
        public string ShiftName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

}
