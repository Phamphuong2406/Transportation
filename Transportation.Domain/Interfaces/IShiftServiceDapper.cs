using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transportation.Infrastructure.Data;

namespace Transportation.Domain.Interfaces
{
    public interface IShiftServiceDapper
    {
        Task<List<Shift>> GetAll();
    }
}
