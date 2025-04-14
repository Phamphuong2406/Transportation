using BusinessLogic.DTOs;
using BusinessLogic.Filter;
using BusinessLogic.Services;
using BusinessLogic.Services.WareHouse;
using DataAccess.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Transportation.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class WarehouseAPIController : ControllerBase
    {
        private IWarehouseService _warehouseService;
        private IShippingRequestService _requestService;
        public WarehouseAPIController(IWarehouseService warehouseService, IShippingRequestService requestService)
        {
            _warehouseService = warehouseService;
            _requestService = requestService;
        }
        [HttpGet("Index")]
        [Authorize("Customer")]
        public IActionResult Index()
        {
            var userIDClaim = User.FindFirst(ClaimTypes.PrimarySid)?.Value;

            // Kiểm tra nếu claim tồn tại và userID hợp lệ
            if (userIDClaim != null)
            {

                var customer = _requestService.GetCustomerByUserId(Convert.ToInt32(userIDClaim));
                if (customer != null)
                {
                    // Lấy danh sách kho của customer này
                    var warehouse = _requestService.GetWarehouseByCutomerId(customer.CustomerId);

                    return Ok(warehouse);
                }
            }
            return BadRequest(new { message = "Vui lòng đăng nhập để thực hiện chức năng!" });
        }

        [HttpPost("CreateWarehouse")]
        [Authorize("Customer")]
        public IActionResult Create([FromBody] WarehouseDto model)
        {
            var userIDClaim = User.FindFirst(ClaimTypes.PrimarySid)?.Value;
            var customer = _warehouseService.GetCustomerById(Convert.ToInt32(userIDClaim));

            if (customer == null)
            {
                ModelState.AddModelError("", "Không tìm thấy thông tin khách hàng.");

            }
            int IdCustomer = customer.CustomerId;
            _warehouseService.Create(model, IdCustomer);

            return Ok(new { success = "Thêm mới thành công" });
        }
   
    }
}
