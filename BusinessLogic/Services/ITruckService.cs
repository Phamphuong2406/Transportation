using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Services.Account;
using DataAccess.DataContext;
using DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface ITruckService
    {
        Truck GetTruck_ById(int TruckId);
        List<Truck> GetAllTruck();
        int Create(TruckDTO model);
        int Detele(int? Truckid);
        TruckDTO GettruckIdByuserId(int userId);
    }
    public class TruckService : ITruckService
    {
        private MyDbContext _context;
        private readonly IMapper _mapper;
        private IAccountService _accountService;
        public TruckService(MyDbContext context, IMapper mapper, IAccountService accountService)
        {
            _context = context;
            _mapper = mapper;
            _accountService = accountService;
        }
        public Truck GetTruck_ById(int TruckId)
        {
            var truck = _context.Trucks.FirstOrDefault(x => x.TruckId == TruckId);
            return truck;
        }
        public List<Truck> GetAllTruck()
        {

            return _context.Trucks.ToList();
        }
        public int Create(TruckDTO model)
        {
            var data = _mapper.Map<Truck>(model);
            _context.Trucks.Add(data);
            return _context.SaveChanges();

        }
        public int Detele(int? Truckid)
        {
            var truck = _context.Trucks.FirstOrDefault(s => s.TruckId == Truckid);
            if (truck != null)
            {
                _context.Trucks.Remove(truck);
                return _context.SaveChanges();
            }
            return -1;
        }
        public TruckDTO GettruckIdByuserId(int userId)
        {
            var driver = _accountService.GetDriverByUserId(userId);
            if (driver != null)
            {
                // Truy vấn trực tiếp trong SQL, chỉ chọn các trường cần thiết
                var truck = _context.Trucks
                    .Where(x => x.DriverId == driver.DriverId)
                    .Select(x => new TruckDTO
                    {
                        TruckId = x.TruckId,
                        DriverId = x.DriverId // Thêm DriverId nếu cần
                    })
                    .FirstOrDefault(); // Lấy phần tử đầu tiên nếu có
                return truck;
            }
            return null;
        }

    }
}
