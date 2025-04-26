using BusinessLogic.DTOs;
using BusinessLogic.Services;
using DataAccess.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Transportation.Areas.Coordinator.Controllers.CoordinationController;

namespace Transportation.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DispatchAPIController : ControllerBase
    {
        private IDispatchService _dispatchService;
        private IShippingRequestService _RequestService;
        private ITripService _tripService;
        private IFileService _fileService;
        public DispatchAPIController(IDispatchService dispatchService, IShippingRequestService requestService, ITripService TripService, IFileService fileService)
        {
            _dispatchService = dispatchService;
            _RequestService = requestService;
            _tripService = TripService;
            _fileService = fileService;
        }
        [HttpGet("GetUnassignedRequests")]
        public IActionResult GetUnassignedRequests()
        {

            return Ok(_dispatchService.GetUnassignedRequests());
        }
        [HttpGet("GetByAssignmentId")]
        public IActionResult GetByIAssignmentId(int assignmentId)
        {

            return Ok(_dispatchService.getByAssignmentId(assignmentId));
        }
        [HttpPost("ProcessRow")]
        public IActionResult ProcessRow([FromBody] AssignmentRequest rq)
        {

            //đơn hàng tương ứng
            var data = _RequestService.getRequestById(rq.Id);

            if (data != null)
            {
                // chuyến hàng có trạng thái phù hợp
                var availableTrips = _tripService.GetTripByStatusandStatus(data);
                if (availableTrips != null)
                {
                    return Ok(availableTrips);
                }
                return NotFound();

            }

            return NotFound(); // Nếu không tìm thấy đơn hàng
        }
        [HttpGet]
        public async Task<IActionResult> Filter(DateOnly keyword)
        {

            return Ok(_RequestService.getRequestBydate(keyword));
        }
        [HttpPost("CreateCoordination")]
        public IActionResult CreateCoordination([FromBody] AssignTripRequestDTO requestt)
        {

            var userId = User.FindFirst(ClaimTypes.PrimarySid)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var result = _dispatchService.AssignTrip(requestt, int.Parse(userId));
            if (result == "Phân công thành công")
            {
                _dispatchService.UpdateStatusById(requestt.RequestId);
                return Ok(new { message = result });
            }
            return BadRequest(new { message = result });


        }
        [HttpGet("ProcessRowTrip")]
        public IActionResult ProcessRowTrip(int tripId)
        {
            // tìm ycvc thuộc chuyến hàng
            var dispatch = _dispatchService.getDispatByTripId(tripId);
            return Ok(dispatch);
        }

        [HttpPost("UpdateStatus")]
        [Consumes("multipart/form-data")]
        public async Task<bool> UpdateStatus([FromForm] UpdateStatusRequest request)
            {
                if (request != null)
                {

                var requestt = _dispatchService.UpdateStatusById(request.RequestId);
                    
                    if (requestt == 1)
                    {
                        return true;
                    }
                }
                return false;
            }
            [HttpPost("Vehicleroute")]
            public IActionResult Vehicleroute([FromBody] GetTripId model)
            {

                if (model != null)
                {
                    // timf danh sách các ycvc của xe
                    var request = _dispatchService.getDispatByTripId(model.TripId);
                    return Ok(request);
                }

                return NotFound();
            }
      

        public class AssignmentRequest
        {
            public int Id { get; set; }
        }
        /* [HttpPost("GetByIDRequest")]
           public IActionResult GetByIDRequest(int Requestid)
           {
               var Request = _context.ShippingRequests.Find(Requestid);
               if (Request != null)
               {
                   //  return Content($"PickupLocation: {Request.PickupLocation}, DropoffLocation: {Request.DropoffLocation}, ShippingCost: {Request.ShippingCost}");
                   return Ok(new
                   {
                       PickupLocation = Request.PickupLocation ?? "Không có dữ liệu",
                       DropoffLocation = Request.DropoffLocation ?? "Không có dữ liệu",
                       ShippingCost = Request.ShippingCost,
                       Weight = Request.Weight,
                       RequestDate = Request.RequestDate,
                       PickupLat = Request.PickupLat,
                       PickupLng = Request.PickupLng,
                       DropoffLat = Request.DropoffLat,
                       DropoffLng = Request.DropoffLng,
                   });
               }
               return NotFound();
           }
           [HttpPost("GetByIDTruck")]
           public IActionResult GetByIDTruck(int Truckid)
           {
               var Truck = _context.Trucks.Find(Truckid);
               if (Truck != null)
               {

                   return Ok(new
                   {
                       ParkingLat = Truck.ParkingLat,
                       ParkingLng = Truck.ParkingLng,
                       Capacity = Truck.Capacity,
                       FuelType = Truck.FuelType,
                   });
               }
               return BadRequest();
           }*/

        [HttpPut("StartShippingRequest")] // dùng Unitofwwork

        public async Task<IActionResult> StartShippingRequest([FromBody] UpdateStatusRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }
            try
            {
                // Tìm Assignment theo RequestId
                var assignment = await _dispatchService.StartShipping(request.RequestId);

                if (assignment == -1)
                {
                    return NotFound(new { message = "Không tìm thấy đơn hàng với mã yêu cầu." });
                }
                var shipping = await _RequestService.StartShipping(request.RequestId);
                if (shipping == -1)
                {
                    return NotFound(new { message = "Không tìm thấy đơn hàng tương ứng" });
                }
                
                return Ok(new { message = "Đã nhận hàng thành công." });
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi cập nhật trạng thái.", error = ex.Message });
            }


        }
       
        [HttpPost("UploadImgAndStatus")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImgAndStatus([FromForm] UploadRequest model)
        {
            var result = await _dispatchService.UploadImageAndUpdateStatus(model.RequestId, model.ImageUpload);

            if (result != "Cập nhật thành công")
            {
                return BadRequest();
            }


            return Ok(new { message = "Cập nhật thành công" });
        }
        public class UploadRequest
        {
            public int RequestId { get; set; }
            public IFormFile ImageUpload { get; set; }
        }


    }
}
