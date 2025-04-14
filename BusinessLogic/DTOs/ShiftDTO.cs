using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class ShiftDTO
    {
        public int ShiftId { get; set; }  // Nếu = 0 thì là tạo mới, ngược lại là cập nhật
        public string ShiftName { get; set; }
        public TimeOnly  StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }

}
