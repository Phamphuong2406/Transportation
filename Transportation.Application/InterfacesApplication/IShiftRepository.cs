using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transportation.Application.DTO;
using Transportation.Infrastructure.Data;

namespace Transportation.Application.InterfacesApplication
{
   
    public interface IShiftRepository
    {
        List<Shift> GetAll();
        int Create_Edit(ShiftDTO shift);
        void Delete(int shiftId);
    }

}
