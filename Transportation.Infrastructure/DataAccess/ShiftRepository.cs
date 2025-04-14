using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transportation.Application.DTO;
using Transportation.Application.InterfacesApplication;
using Transportation.Infrastructure.Data;

namespace Transportation.Infrastructure.DataAccess
{
    public class ShiftRepository : IShiftRepository
    {
        private MyDbContext _context;
        private readonly IMapper _mapper;
        public ShiftRepository(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // getAll
        public List<Shift> GetAll()
        {

            return _context.Shifts.ToList();
        }
        //thêm
        public int Create_Edit(ShiftDTO shiftDto)
        {
            var shiftEntity = _mapper.Map<Shift>(shiftDto); // Ánh xạ từ DTO → Entity

            try
            {
                if (shiftEntity.ShiftId <= 0)
                {
                    _context.Shifts.Add(shiftEntity);

                }
                else
                {
                    var Shift = _context.Shifts.Find(shiftEntity.ShiftId);
                    if (Shift != null || Shift.ShiftId < 1)
                    {
                        return -2;
                    }
                    _context.Shifts.Update(shiftEntity);
                   
                }
                return _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }


        }
        //xóa
        public void Delete(int shiftId)
        {
            var Id = _context.Shifts.Find(shiftId);
            if (Id != null)
            {
                _context.Remove(Id);
                _context.SaveChanges();
            }
        }

      
    }
}
