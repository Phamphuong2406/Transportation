using BusinessLogic.Services;
using BusinessLogic.Services.Account;
using DataAccess.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Transportation.Areas.Drivers.Controllers.HomeDriverController;

namespace Transportation.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripAPIController : ControllerBase
    {
        private ITripService _tripService;
        private IAccountService _accountService;
        public TripAPIController(ITripService tripService, IAccountService accountService)
        {
            _tripService = tripService;
            _accountService = accountService;
        }
        [HttpGet("GetAllTrip")]
        public IActionResult GetAllTrip()
        {
            var trips = _tripService.GetAllTrip();
            return Ok(trips);
        }
        [HttpPost("CreateTrip")]
        public IActionResult CreateTrip([FromForm] Trip model)
        {
            if (ModelState.IsValid)
            {
                _tripService.CreateTrip(model);
                return Ok();
            }
            return BadRequest();
        }
        [HttpGet("GetTripByDriverId")]
        public IActionResult GetTripByDriverId()
        {
            var userID = User.FindFirst(ClaimTypes.PrimarySid)?.Value;
            // Kiểm tra nếu claim tồn tại và userID hợp lệ
            if (userID == null)
            {
                return NotFound();
            }
            var listTrip = _tripService.GetTripByUserId(Convert.ToInt32(userID));
            if (listTrip == null)
            {
                return NotFound();
            }
            return Ok(listTrip);

        }
        [HttpPut("StopSharing")]
        public IActionResult StopSharing([FromBody] updateLocation model)
        {
            // thay đổi trạng thái chuyến  hàng
           var result = _tripService.EndTrip(model.tripId);
            if (result == 1) {
                return Ok(new { success = true, message = "Đã cập nhật trạng thái thành công" });
            }

            return NotFound();
        }
    }
}
