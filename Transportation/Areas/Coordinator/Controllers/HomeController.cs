using DataAccess.DataContext;
using DataAccess.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Transportation.Areas.Coordinator.Controllers
{
    [Area("Coordinator")]

    [Authorize(Roles = "Dispatcher")]
    public class HomeController : Controller
    {
        private MyDbContext _context;
        public HomeController(MyDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var userName = User.Identity.IsAuthenticated ? User.Identity.Name : "Khách";
            ViewData["UserName"] = userName;
            return View();
        }

        public IActionResult GetList()
        {
            var data = _context.Trucks.Select(tr => new Truck
            {
                TruckId = tr.TruckId,
                DriverId = tr.DriverId,
                Capacity = tr.Capacity,
                FuelType = tr.FuelType,
                ParkingLocation = tr.ParkingLocation
            }).ToList();
            return Ok(data);
        }

        [HttpGet]
        /* public IActionResult TruckLoadDistribution() // phân bổ trọng tải
         {
             var loadData = _context.Trucks
                 .Select(truck => new
                 {
                     TruckId = truck.TruckId,

                     UsedLoad = _context.DispatchAssignments.Where(d => d.Trip.TruckId == truck.TruckId).Sum(d => d.Weight), // Tải trọng đã sử dụng
                     MaxLoad = truck.Capacity // Tải trọng tối đa
                 })
                 .ToList();

             return Json(loadData);
         }*/

        public IActionResult TruckLoadDistribution(int year, int month, int day) // tổng trọng tải sưr dụng
        {
            var query = _context.DispatchAssignments
                .Where(d => d.AssignedDate.Year == year && d.AssignedDate.Month == month );

            // Nếu có chọn ngày, lọc theo ngày
            if (day > 0)
            {
                query = query.Where(d => d.AssignedDate.Day == day);
            }

            var loadData = query
                .GroupBy(d => d.Trip.TruckId)
                .Select(g => new
                {
                    TruckId = g.Key,
              
                    UsedLoad = g.Sum(d => d.Weight),  // Tổng tải trọng đã dùng
                    MaxLoad = _context.Trucks.Where(t => t.TruckId == g.Key).Select(t => t.Capacity).FirstOrDefault() // Tải trọng tối đa
                })
                .ToList();

            return Json(loadData);
        }



        [HttpGet]
        public JsonResult GetLateDeliveryData() // thống kê đơn hàng trễ
        {
            var totalOrders = _context.DispatchAssignments.Count(a =>a.Status == "Đã giao hàng"); // tổng đơn hàng


            var lateOrders = _context.DispatchAssignments.Include(x => x.Trip)
                .AsEnumerable().Count(o =>
                                      o.Status == "Đã giao hàng" &&
                                      o.Deliverydate.Value.TimeOfDay > o.Trip.EndTime.GetValueOrDefault().ToTimeSpan());

            var data = new
            {
                total = totalOrders,
                late = lateOrders,
                onTime = totalOrders - lateOrders
            };

            return Json(data);
        }

        [HttpGet]
        public IActionResult GetDriverPerformanceData() // thống kê hiêju suất xe tải
        {
            try
            {
                var driverPerformance = _context.Trucks
                    .Include(x => x.Trips).ThenInclude(t => t.DispatchAssignments)
                    .Include(x => x.Driver)
                    .AsEnumerable()
                    .Where(t => t.Driver != null) // Đảm bảo có tài xế
                    .Select(d => new
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

                if (!driverPerformance.Any())
                {
                    return Json(new { message = "Không có dữ liệu." }); // Trả về JSON hợp lệ thay vì HTTP 204
                }

                return Json(driverPerformance);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi server: " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult TruckOrderStatistics(int year, int month) // tổng số đơn hàng của mỗi xe tải
        {
            var query = _context.DispatchAssignments
                .Where(d => d.AssignedDate.Year == year && d.AssignedDate.Month == month);

            var orderData = query
                .GroupBy(d => d.Trip.TruckId)
                .Select(g => new
                {
                    TruckId = g.Key,
                  
                    TotalOrders = g.Count() // Tổng số đơn hàng mỗi xe tải
                })
                .ToList();

            return Json(orderData);
        }

        [HttpGet]
        public IActionResult OrderStatusStatistics(int year, int month)
        {
            var query = _context.DispatchAssignments
                .Where(d => d.AssignedDate.Year == year && d.AssignedDate.Month == month);

            var statusData = query
                .GroupBy(d => d.Status)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToList();

            return Json(statusData);
        }



    }
}


