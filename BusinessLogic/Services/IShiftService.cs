using BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IShiftService
    {
        List<ShiftDTO> GetAllShifts();

        // Thêm hoặc cập nhật shift
        int CreateOrUpdateShift(ShiftDTO shiftDTO);

        // Xóa shift
        void DeleteShift(int shiftId);
        ShiftDTO GetShiftById(int shiftId);
    }
}
