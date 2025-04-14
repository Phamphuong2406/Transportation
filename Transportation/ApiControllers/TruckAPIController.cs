using BusinessLogic.DTOs;
using BusinessLogic.Services;
using DataAccess.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Transportation.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TruckAPIController : ControllerBase
    {
        private ITruckService _truckService;
        public TruckAPIController(ITruckService truckService)
        {
            _truckService = truckService;
        }
        [HttpGet("GetTruckById")]
       public IActionResult GetTruckById(int truckId)
        {
            var truck = _truckService.GetTruck_ById(truckId);
            return Ok(truck);
        }
        [HttpGet("GetCurrentTruck")]
        public IActionResult GetCurrentTruck()
        {
            var userID = User.FindFirst(ClaimTypes.PrimarySid)?.Value;
            // Kiểm tra nếu claim tồn tại và userID hợp lệ
            if (userID != null)
            {

                var Truck = _truckService.GettruckIdByuserId(Convert.ToInt32(userID));
                if (Truck != null)
                {
                    return Ok(Truck);
                }
                return NotFound();
            }
            return NotFound();
        }
        [HttpGet("GetAllTruck")]
        public IActionResult GetAllTruck()
        {
            return Ok(_truckService.GetAllTruck());
        }

        [HttpPost("CreateTruck")]
        public IActionResult CreateTruck([FromForm] TruckDTO model)
        {
            if (ModelState.IsValid)
            {
                var truck = _truckService.Create(model);

                return Ok();
            }

            return BadRequest();
        }
       /* [HttpPost]
        public IActionResult EditTruck(Truck model)
        {
            var truck = _context.Trucks.Include(x => x.Driver).ToList();
            return View(truck);
        }*/
        [HttpDelete("DeleteTruck")]
        public IActionResult DeleteTruck(int? truckId)
        {
           var data = _truckService.Detele(truckId);
            return Ok();
        }
    }
}
