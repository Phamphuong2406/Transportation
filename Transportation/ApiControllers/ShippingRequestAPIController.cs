using BusinessLogic.DTOs;
using BusinessLogic.Filter;
using BusinessLogic.Services;
using DataAccess.Entity;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Transportation.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingRequestAPIController : ControllerBase
    {
        private IShippingRequestService _shippingRequestService;
        public ShippingRequestAPIController(IShippingRequestService shippingRequestService)
        {
            _shippingRequestService = shippingRequestService;
        }

        [HttpGet("GetWarehouse")]
        [Authorize("Customer")]
        public IActionResult Index()
        {

            var userIDClaim = User.FindFirst(ClaimTypes.PrimarySid)?.Value;
            // Kiểm tra nếu claim tồn tại và userID hợp lệ
            if (userIDClaim != null && long.TryParse(userIDClaim, out long userID))
            {

                var customer = _shippingRequestService.GetCustomerByUserId(Convert.ToInt32(userID));
                if (customer != null)
                {
                    // Lấy danh sách kho của customer này
                    var warehouse = _shippingRequestService.GetWarehouseByCutomerId(customer.CustomerId);

                    return Ok(warehouse);
                }
            }
            return BadRequest(new { message = "Vui lòng đăng nhập để thực hiện chức năng!" });


        }

        [HttpGet("GetShippingRequets")]
        [Authorize("Customer")]

        public IActionResult GetShipping()
        {
            var userId = User.FindFirst(ClaimTypes.PrimarySid)?.Value;
            // Kiểm tra nếu claim tồn tại và userID hợp lệ
            if (userId != null && long.TryParse(userId, out long userID))
            {

                var customer = _shippingRequestService.GetCustomerByUserId(Convert.ToInt32(userID));
                if (customer != null)
                {
                    // Lấy danh sách YCVC của customer này
                    var Request = _shippingRequestService.GetShippingRByCutomerId(customer.CustomerId);


                    return Ok(Request);
                }
            }
            return BadRequest(new { message = "Vui lòng đăng nhập để thực hiện chức năng!" });

        }

        [HttpPut("DeleteRequest")]

        public IActionResult DeleteRequest(int id)
        {
            _shippingRequestService.DeleteShippingById(id);
            return Ok();
        }
        [HttpPut("ConfirmReceived")]
        public IActionResult GetRequest(int id)
        {
            _shippingRequestService.ConfirmReceivedById(id);
            return Ok();
        }
        [HttpGet("GetAllProductType")]
        public IActionResult GetAll()
        {
            var productTypes = _shippingRequestService.GetAllProductTypes();

            return Ok(productTypes);
        }
        [HttpPost("CalculateShippingCost")]
        [Consumes("multipart/form-data")]
        public decimal CalculateShippingCost([FromForm] float distance, [FromForm] float weight, [FromForm] int productTypeId)
        {
            // Bước 2: Đặt các thông số phí cơ bản
            decimal basePrice = 50000; // Phí cơ bản
            decimal pricePerKm = 5000; // Giá mỗi km
            decimal pricePerKg = 2000; // Giá mỗi kg

            // Bước 3: Điều chỉnh phí theo loại hàng hóa
            switch (productTypeId)
            {
                case 1: // Hàng dễ vỡ
                    basePrice *= 1.2m; // Tăng phí cơ bản 20%
                    pricePerKg *= 1.5m; // Tăng phí mỗi kg 50%
                    break;

                case 2: // Hàng cồng kềnh
                    pricePerKm *= 1.3m; // Tăng phí mỗi km 30%
                    break;

                case 3: // Hàng thông thường
                        // Không thay đổi, sử dụng các giá trị mặc định
                    break;

                case 4: // Hàng nguy hiểm
                    basePrice *= 1.5m; // Tăng phí cơ bản 50%
                    pricePerKg *= 2m; // Tăng phí mỗi kg gấp đôi
                    break;

                default:
                    throw new ArgumentException("Loại hàng hóa không hợp lệ");
            }
            // Bước 4: Tính tổng phí
            decimal shippingCost = basePrice + (pricePerKm * (decimal)distance) + (pricePerKg * (decimal)weight);
            return shippingCost;
        }


        [HttpPost("CreateRequest")]
        [Authorize("Customer")]
        public IActionResult Create([FromBody] ShippingRequetsDTO model)
        {

            var user = User.FindFirst(ClaimTypes.PrimarySid)?.Value;
            _shippingRequestService.Create(model, Convert.ToInt32(user));
            return Ok();
        }

        [HttpGet("GetRoute")]
        public IActionResult GetRoute(int id)
        {
            var request =_shippingRequestService.getRequestById(id);
            if (request == null) return NotFound();

            return Ok(request);
        }

    }

}

