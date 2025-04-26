using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Services.Account;
using DataAccess.DataContext;
using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;
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
        List<TruckOrderStatisticDTO> GetTruckOrderStatistics(int year, int month);
        List<DriverPerformanceDTO> GetDriverPerformanceData();
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
        public List<TruckOrderStatisticDTO> GetTruckOrderStatistics(int year, int month)
        {
            var query = _context.DispatchAssignments
             .Where(d => d.AssignedDate.Year == year && d.AssignedDate.Month == month);

            var orderData = query
                .GroupBy(d => d.Trip.TruckId)
                .Select(g => new TruckOrderStatisticDTO
                {
                    TruckId = g.Key,
                    TotalOrders = g.Count() // Tổng số đơn hàng mỗi xe tải
                })
                .ToList();
          
            return orderData;
        }
        public List<DriverPerformanceDTO> GetDriverPerformanceData()
        {
            var driverPerformance = _context.Trucks
                  .Include(x => x.Trips).ThenInclude(t => t.DispatchAssignments)
                  .Include(x => x.Driver)
                  .AsEnumerable()
                  .Where(t => t.Driver != null) // Đảm bảo có tài xế
                  .Select(d => new DriverPerformanceDTO
                  {
                      DriverName = d.Driver.FullName ?? "Không có tài xế",
                      TotalTrips = d.Trips.Count(x => x.Status == "Hoàn thành"),
                      TotalOrders = d.Trips.Sum(t => t.DispatchAssignments.Count(u => u.Status == "Đã giao hàng")),
                      // ct: ontimerate = ( số đơn hàng giao đúng hạn / tổng số đơn hàng) * 100
                      OnTimeRate = d.Trips.Sum(t => t.DispatchAssignments.Count(u => u.Status == "Đã giao hàng")) > 0
                          ? (double)d.Trips.Sum(t => t.DispatchAssignments.Count(o => o.Deliverydate.HasValue
                              && o.Deliverydate.Value.TimeOfDay <= t.EndTime.GetValueOrDefault().ToTimeSpan()))
                            / d.Trips.Sum(t => t.DispatchAssignments.Count(u => u.Status == "Đã giao hàng")) * 100
                          : 0
                  })
                  .ToList();
            return driverPerformance;
        }
    }
}
