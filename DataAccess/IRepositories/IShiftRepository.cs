using DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace DataAccess.IRepositories
{
    public interface IShiftRepository
    {
        List<Shift> GetAll();
        int Create_Edit(Shift shift);
        void Delete(int shiftId);

    }
}
