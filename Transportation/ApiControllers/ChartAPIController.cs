using BusinessLogic.Filter;
using BusinessLogic.Services;
using DataAccess.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Transportation.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartAPIController : ControllerBase
    {
        private readonly ITruckService _truckService;
        private readonly IDispatchService _dispatchService;
        public ChartAPIController(ITruckService truckService, IDispatchService dispatchService)
        {
            _truckService = truckService;
            _dispatchService = dispatchService;
        }
        [HttpGet("TruckOrderStatistics")]
        [Authorize("Dispatcher")]
        public IActionResult TruckOrderStatistics(int year, int month) // tổng số đơn hàng của mỗi xe tải
        {
            var data = _truckService.GetTruckOrderStatistics(year, month);
            return Ok(data);
        }

        [HttpGet("OrderStatusStatistics")]
        [Authorize("Dispatcher")]
        public IActionResult OrderStatusStatistics(int year, int month)
        {
            var data = _dispatchService.GetOrderStatusStatistics(year, month);

            return Ok(data); ;
        }

        [HttpGet("GetLateDeliveryData")]
        [Authorize("Dispatcher")]
        public IActionResult GetLateDeliveryData(int year, int month) // thống kê đơn hàng trễ
        {
           var data = _dispatchService.GetLateDeliveryData(year, month);
            return Ok(data);
        }

        [HttpGet("DriverPerformanceData")]
        [Authorize("Dispatcher")]
        public IActionResult DriverPerformanceData(int year, int month) // thống kê hiêju suất xe tải
        {
            try
            {
                var data = _truckService.GetDriverPerformanceData(year,month);

                if (!data.Any())
                {
                    return NotFound(new { message = "Không có dữ liệu." }); 
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi server: " + ex.Message });
            }
        }
        /* [HttpGet("GetList")]
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
         }*/

   
        [HttpGet("TruckLoadDistribution")]
        public IActionResult TruckLoadDistribution(int year, int month) // tổng trọng tải sưr dụng
        {
           
            var data = _dispatchService.GetTruckLoadDistribution(year, month);
            return Ok(data);
        }
        // so sánh khối lượng đơn hàng của các xe
        [HttpGet("Compareordersoftruck")]
        [Authorize("Admin")]
        public IActionResult Compareordersoftruck(int month)
        {
            var result = _dispatchService.GetCompareordersoftruck(month);
            return Ok(result);
        }
        [HttpGet("CompareRevenue")]
        [Authorize("Admin")]
        // tổng doanh thu theo tháng
        public IActionResult CompareRevenue()
        {
            // lấy dữ liệu của bảng
            var data = _dispatchService.GetCompareRevenue();
            return Ok(data);
        }
        [HttpGet("Compareorders")]
        [Authorize("Admin")]
        //trạng thái đơn hàng
        public IActionResult Compareorders()
        {
            var data = _dispatchService.GetCompareorders();
            return Ok(data);
        }
        [HttpGet("CargoWeightChart")]
        [Authorize("Admin")]
        // Tổng khối lượng hàng hóa
        public IActionResult CargoWeightChart()
        {
            var result = _dispatchService.GetCargoWeightChart();
            return Ok(result);
        }
    }
}
