using AutoMapper;
using DataAccess.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace Transportation.Areas.Admin.Controllers
{
    [Area("Admin")]
  //  [Authorize(Roles = "Admin")]
    public class HomeAdminController : Controller
    {
        private MyDbContext _context;

        public HomeAdminController(MyDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            var userName = User.Identity.IsAuthenticated ? User.Identity.Name : "Khách";
            ViewData["UserName"] = userName;
            return View();
        }
        // so sánh khối lượng đơn hàng của các xe
        public IActionResult Compareordersoftruck(int? month)
        {
            var data = _context.DispatchAssignments.Where(t => t.RequestDate.Month == month).
                Select(x => new
                {
                    TruckId = x.Trip.TruckId,
                    Status = x.Status,

                }).ToList();
            var result = data.Where(x => x.Status == "Đã giao hàng")
                .GroupBy(x => x.TruckId)
                .Select(item => new
                {
                    Truckid = item.Key,
                    TotalProcessed = item.Count()
                });
            return Json(result);
        }

        // tổng doanh thu theo tháng
        public IActionResult CompareRevenue()
        {
            // lấy dữ liệu của bảng
            var data = _context.DispatchAssignments.ToList();

            var resultt = data.Where(x => x.Status == "Đã giao hàng")
               .GroupBy(x => x.RequestDate.Month)
               .Select(item => new
               {
                   Date = item.Key,
                   Cost = item.Sum(x => x.ShippingCost)
               });
            return Json(resultt);
        }

        //trạng thái đơn hàng
        public IActionResult Compareorders()
        {
            var orderStatusCounts = _context.DispatchAssignments
            .GroupBy(o => o.Status)
            .Select(g => new
            {
                Status = g.Key,
                Count = g.Count()
            })
            .ToList();
            return Json(orderStatusCounts);
        }

        // Tổng khối lượng hàng hóa
        public IActionResult CargoWeightChart()
        {
            var weightData = _context.DispatchAssignments
                .Where(o => o.Status == "Đã giao hàng" || o.Status == "Đã nhận hàng") // Lọc theo trạng thái
                .GroupBy(o => o.RequestDate.Month)  // Nhóm theo ngày
                .Select(g => new
                {
                    Date = g.Key,
                    TotalWeight = g.Sum(o => o.Weight)
                })

                .ToList();
            return Json(weightData);
        }



        /* Biểu đồ chi phí vận hành
 Loại biểu đồ: Đường hoặc tròn.
 Mục đích: Theo dõi các khoản chi phí như nhiên liệu, bảo dưỡng xe, lương tài xế, v.v.
 Thông tin hiển thị: Phân tích chi phí theo danh mục hoặc thời gian.*/


    }
}


