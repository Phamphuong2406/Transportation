using AutoMapper;
using BusinessLogic.DTOs;
using DataAccess.DataContext;
using DataAccess.Entity;
using DataAccess.IRepositories;
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
    public class ShiftService : IShiftService
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IMapper _mapper;
        private MyDbContext _context;
        public ShiftService(IShiftRepository shiftRepository, MyDbContext context, IMapper mapper)
        {
            _shiftRepository = shiftRepository;
            _context = context;
            _mapper = mapper;
        }

        // Lấy tất cả shift (trả về danh sách DTO)
        public List<ShiftDTO> GetAllShifts()
        {
            var shifts = _shiftRepository.GetAll();
            return _mapper.Map<List<ShiftDTO>>(shifts); // Đúng cách
        }



        // Thêm hoặc cập nhật shift
        public int CreateOrUpdateShift(ShiftDTO shiftDTO)
        {
            var shift = _mapper.Map<Shift>(shiftDTO);
            return _shiftRepository.Create_Edit(shift);
        }

        // Xóa shift
        public void DeleteShift(int shiftId)
        {
            _shiftRepository.Delete(shiftId);
        }
        public ShiftDTO GetShiftById(int shiftId)
        {
            var data = _context.Shifts.Where(x => x.ShiftId == shiftId)
                .Select(x => new ShiftDTO
                {
                    StartTime = x.StartTime,
                    EndTime = x.EndTime
                }).FirstOrDefault();

            return data;


        }
    }
}
