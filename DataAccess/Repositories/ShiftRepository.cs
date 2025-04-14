using DataAccess.DataContext;
using DataAccess.Entity;
using DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccess.Repositories
{
    public class ShiftRepository : IShiftRepository
    {
        private readonly MyDbContext _context;

        public ShiftRepository(MyDbContext context)
        {
            _context = context;
        }

        // Lấy danh sách tất cả ca làm
        public List<Shift> GetAll()
        {
            return _context.Shifts.ToList();
        }

        

        // Xóa Shift
        public void Delete(int shiftId)
        {
            var shift = _context.Shifts.Find(shiftId);
            if (shift != null)
            {
                _context.Shifts.Remove(shift);
                _context.SaveChanges();
            }
        }

        public int Create_Edit(Shift shift)
        {
            try
            {
                if (shift.ShiftId == 0) // ShiftId == 0 nghĩa là tạo mới
                {
                    _context.Shifts.Add(shift);
                }
                else
                {
                    var existingShift = _context.Shifts.Find(shift.ShiftId);
                    if (existingShift == null) return -2; // Không tìm thấy shift

                    _context.Entry(existingShift).CurrentValues.SetValues(shift);
                }

                return _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
