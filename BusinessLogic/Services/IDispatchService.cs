using Azure.Core;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.Account;
using BusinessLogic.Interfaces;
using DataAccess.DataContext;
using DataAccess.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static BusinessLogic.Services.DispatchService;

namespace BusinessLogic.Services
{
    public interface IDispatchService
    {
        List<ShippingRequetsDTO> GetUnassignedRequests();
        DispatchAssignmentDTO getByAssignmentId(int assignmentId);
        string AssignTrip(AssignTripRequestDTO assignrequest, int userId);
        int UpdateStatusById(int? requestId);
        List<DispatchAssignmentDTO> getDispatByTripId(int tripId);
        List<OrderStatusDTO> GetOrderStatusStatistics(int year, int month);
        LateDeliveryDataDTO GetLateDeliveryData();
        List<TruckLoadDistributionDTO> GetTruckLoadDistribution(int year, int month);
        IEnumerable<CompareordersoftruckDTO> GetCompareordersoftruck(int month);
        IEnumerable<CompareRevenueDTO> GetCompareRevenue();
        List<OrderStatusDTO> GetCompareorders();
        List<CargoWeightChartDTO> GetCargoWeightChart();
        Task<int> StartShipping(int? requestId);
        Task<string> UploadImageAndUpdateStatus(int requestId, IFormFile imageUpload);
    }
    public class DispatchService : IDispatchService
    {
        private MyDbContext _context;
        private IShippingRequestService _RequestService;
        private ITripService _tripService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;
        private IFileService _fileService;
        public DispatchService(MyDbContext context, IShippingRequestService requestService, ITripService tripService, IUnitOfWork unitOfWork, IWebHostEnvironment env, IFileService fileService)
        {
            _context = context;
            _RequestService = requestService;
            _tripService = tripService;
            _env = env;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }
        public List<ShippingRequetsDTO> GetUnassignedRequests()
        {
            var result = _context.ShippingRequests.Include(x => x.Customer).Include(t => t.ProductType)
                .Select(x => new ShippingRequetsDTO
                {
                    RequestId = x.RequestId,
                    CustomerName = x.Customer.FullName,
                    RequestDate = x.RequestDate,
                    ShippingCost = x.ShippingCost,
                    Weight = x.Weight,
                    ProductNameType = x.ProductType.Name,
                    Status = x.Status

                })
                .OrderByDescending(y => y.RequestDate).ToList();
            return result;
        }
        public DispatchAssignmentDTO getByAssignmentId(int assignmentId)
        {
            var assignment = _context.DispatchAssignments.Include(x => x.Trip.Truck)
                                                   .Include(x => x.Request).ThenInclude(y => y.ProductType)
                                                   .Select(x => new DispatchAssignmentDTO
                                                   {
                                                       RequestId = x.RequestId,
                                                       PickupLocation = x.PickupLocation,
                                                       DropoffLocation = x.DropoffLocation,
                                                       Weight = x.Weight,
                                                       ShippingCost = x.ShippingCost,
                                                       Note = x.Note,
                                                       PickupLat = x.PickupLat,
                                                       PickupLng = x.PickupLat,
                                                       DropoffLat = x.DropoffLat,
                                                       DropoffLng = x.DropoffLng,
                                                       ProductTypeId = x.Request.ProductType.ProductTypeId,
                                                       Capacity = x.Capacity
                                                   }
                )
                .SingleOrDefault(x => x.AssignmentId == assignmentId);

            if (assignment == null)
            {
                return null;
            }
            return assignment;
        }

        public string AssignTrip(AssignTripRequestDTO assignrequest, int userId)
        {
            var trip = _tripService.GetTripById(assignrequest.TripId);
            var request = _RequestService.getRequestById(assignrequest.RequestId);
            var dispatcher = _context.Dispatchers.SingleOrDefault(x => x.UserId == userId);
            if (dispatcher != null)
            {
                if (request.Status == "Chờ xác nhận")
                {
                    var assignment = new DispatchAssignment
                    {
                        RequestId = request.RequestId,
                        TripId = trip.TripId,
                        AssignedBy = dispatcher.DispatcherId,
                        AssignedDate = DateOnly.FromDateTime(DateTime.Now),
                        RequestDate = request.RequestDate,
                        PickupLocation = request.PickupLocation,
                        PickupLat = request.PickupLat,
                        PickupLng = request.PickupLng,
                        DropoffLocation = request.DropoffLocation,
                        DropoffLat = request.DropoffLat,
                        DropoffLng = request.DropoffLng,
                        Weight = request.Weight,
                        ProductTypeId = request.ProductTypeId,
                        ShippingCost = request.ShippingCost,
                        ParkingLat = trip.Truck.ParkingLat,
                        ParkingLng = trip.Truck.ParkingLng,
                        Capacity = trip.Truck.Capacity,
                        FuelType = trip.Truck.FuelType,
                        Pickupdate = request.Pickupdate,
                        //Deliverydate = request.Deliverydate, 
                        Deliverydate = null,
                        Note = request.Note,

                        Status = "Đã gán xe"

                    };

                    _context.DispatchAssignments.Add(assignment);
                    trip.Status = "Đã gán xe";

                    _context.SaveChanges();
                    // Xử lý logic điều xe ở đây
                    // Ví dụ: Gán xe cho đơn hàng

                    return "Phân công thành công";
                }
                else
                {
                    return "YCVC đã được điều phối hoặc đã bị hủy. Vui lòng chọn lại";

                }
            }
            return "người dùng không tồn tại";
        }

        public int UpdateStatusById(int? requestId)
        {
            var requestt = _context.ShippingRequests.Find(requestId);
            if (requestt != null)
            {
                requestt.Status = "Đã điều phối xe";
                _context.ShippingRequests.Update(requestt);
                _context.SaveChanges();
                return 1;
            }
            return -1;
        }
        public async Task<int> StartShipping(int? requestId)
        {
            var assignment = await _context.DispatchAssignments.FirstOrDefaultAsync(a => a.RequestId == requestId);

            if (assignment != null)
            {
                assignment.Status = "Đã lấy hàng";
                assignment.Pickupdate = DateTime.Now;
                _context.DispatchAssignments.Update(assignment);
                _context.SaveChanges();
                _tripService.StartShipping(assignment.TripId);
                return 1;
            }
            return -1;
        }
        public async Task<int> EndShipping(int? requestId)
        {
            var assignment = await _context.DispatchAssignments.FirstOrDefaultAsync(a => a.RequestId == requestId);

            if (assignment != null)
            {
                assignment.Status = "Đã lấy hàng";
                assignment.Pickupdate = DateTime.Now;
                _context.DispatchAssignments.Update(assignment);
                _context.SaveChanges();
                _tripService.StartShipping(assignment.TripId);
                return 1;
            }
            return -1;
        }
        public List<DispatchAssignmentDTO> getDispatByTripId(int tripId)
        {
            // timf danh sách các ycvc của xe
            var listdispatch = _context.DispatchAssignments.Where(x => x.TripId == tripId).Select(t => new DispatchAssignmentDTO
            {
                PickupLat = t.PickupLat,
                PickupLng = t.PickupLng,
                DropoffLat = t.DropoffLat,
                DropoffLng = t.DropoffLng,
                PickupLocation = t.PickupLocation,
                RequestId = t.RequestId,
                DropoffLocation = t.DropoffLocation,
                Weight = t.Weight,
                ShippingCost = t.ShippingCost,
                Status = t.Status,
                AssignmentId = t.AssignmentId
            }).ToList();
            return listdispatch;
        }

       public List<OrderStatusDTO> GetOrderStatusStatistics(int year, int month)
        {
            var query = _context.DispatchAssignments
               .Where(d => d.AssignedDate.Year == year && d.AssignedDate.Month == month);

            var statusData = query
                .GroupBy(d => d.Status)
                .Select(g => new OrderStatusDTO
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToList();
            return statusData;
        }
        public LateDeliveryDataDTO GetLateDeliveryData()
        {
            var totalOrders = _context.DispatchAssignments.Count(a => a.Status == "Đã giao hàng"); // tổng đơn hàng


            var lateOrders = _context.DispatchAssignments.Include(x => x.Trip)
                .AsEnumerable().Count(o =>
                                      o.Status == "Đã giao hàng" &&
                                      o.Deliverydate.Value.TimeOfDay > o.Trip.EndTime.GetValueOrDefault().ToTimeSpan());
            var data = new LateDeliveryDataDTO
            {
                total = totalOrders,
                late = lateOrders,
                onTime = totalOrders - lateOrders
            };
            return data;
        }
        public List<TruckLoadDistributionDTO> GetTruckLoadDistribution(int year, int month)
        {
            var query = _context.DispatchAssignments
               .Where(d => d.AssignedDate.Year == year && d.AssignedDate.Month == month);
            var loadData = query
                .GroupBy(d => d.Trip.TruckId)
                .Select(g => new TruckLoadDistributionDTO
                {
                    TruckId = g.Key,
                    UsedLoad = g.Sum(d => d.Weight),  // Tổng tải trọng đã dùng
                    MaxLoad = _context.Trucks.Where(t => t.TruckId == g.Key).Select(t => t.Capacity).FirstOrDefault() // Tải trọng tối đa
                })
                .ToList();
            return loadData;
        }
        public IEnumerable<CompareordersoftruckDTO> GetCompareordersoftruck(int month)
        {
            var data = _context.DispatchAssignments.Where(t => t.RequestDate.Month == month).
                Select(x => new CompareordersoftruckDTO
                {
                    TruckId = x.Trip.TruckId,
                    Status = x.Status,

                }).ToList();
            var result = data.Where(x => x.Status == "Đã giao hàng")
                .GroupBy(x => x.TruckId)
                .Select(item => new CompareordersoftruckDTO
                {
                    TruckId = item.Key,
                    TotalProcessed = item.Count()
                });
            return result;
        }
       public IEnumerable<CompareRevenueDTO> GetCompareRevenue()
        {
            var data = _context.DispatchAssignments.ToList();

            var resultt = data.Where(x => x.Status == "Đã giao hàng")
               .GroupBy(x => x.RequestDate.Month)
               .Select(item => new CompareRevenueDTO
               {
                   Date = item.Key,
                   Cost = item.Sum(x => x.ShippingCost)
               });
            return resultt;
        }
        public List<OrderStatusDTO> GetCompareorders()
        {
            var orderStatusCounts = _context.DispatchAssignments
           .GroupBy(o => o.Status)
           .Select(g => new OrderStatusDTO
           {
               Status = g.Key,
               Count = g.Count()
           })
           .ToList();
            return orderStatusCounts;
        }
        public List<CargoWeightChartDTO> GetCargoWeightChart()
        {
            var weightData = _context.DispatchAssignments
                .Where(o => o.Status == "Đã giao hàng" || o.Status == "Đã nhận hàng") // Lọc theo trạng thái
                .GroupBy(o => o.RequestDate.Month)  // Nhóm theo ngày
                .Select(g => new CargoWeightChartDTO
                {
                    Date = g.Key,
                    TotalWeight = g.Sum(o => o.Weight)
                })

                .ToList();
            return weightData;
        }
        public async Task<string> UploadImageAndUpdateStatus(int requestId, IFormFile imageUpload)
        {
            if (requestId <= 0 || imageUpload == null || imageUpload.Length == 0)
            {
                return "Thông tin không hợp lệ";
            }

            // Tìm assignment theo RequestId
            var assignment = await _unitOfWork.Assignments.GetByRequestId(requestId);

            if (assignment == null)
            {
                return "Không tìm thấy đơn hàng !";
            }

            var shipping = await _unitOfWork.ShippingRequests.GetByRequestId(requestId);

            if (shipping == null)
            {
                return "Không tìm thấy đơn vận đơn";
            }

            if (imageUpload?.Length > 1 * 1024 * 1024)
            {
                return "Kích cỡ file 1 MB";
            }
            string[] allowedFileExtentions = [".jpg", ".jpeg", ".png"];
            string createdImageName = await _fileService.SaveFileAsync(imageUpload, allowedFileExtentions);

            var trip = await _context.Trips.FirstOrDefaultAsync(x => x.TripId == assignment.TripId);
            if (trip == null)
            {
                return "Không tìm thấy chuyến hàng ";
            }

            // Cập nhật thông tin giao hàng và trạng thái
            assignment.DeliveryImage = createdImageName;
            assignment.Deliverydate = DateTime.Now;
            assignment.Status = "Đã giao hàng";
            shipping.Status = "Đã giao hàng";
            trip.Status = "Hoàn thành";

            await _unitOfWork.SaveAsync();
            return "Cập nhật thành công";

        }
    }
}
