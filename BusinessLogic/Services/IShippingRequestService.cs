using AutoMapper;
using BusinessLogic.DTOs;
using DataAccess.DataContext;
using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace BusinessLogic.Services
{
    public interface IShippingRequestService
    {
        Customer GetCustomerByUserId(int id);
        List<WarehouseDtO> GetWarehouseByCutomerId(int customerId);
        List<ShippingRequetsDTO> GetShippingRByCutomerId(int customerId);
        void DeleteShippingById(int IdRequest);
        void ConfirmReceivedById(int IdRequest);
        List<ProductTypeDto> GetAllProductTypes();
        void Create(ShippingRequetsDTO model, int userId);
        ShippingRequest getRequestById(int requestId);
        List<ShippingRequest> getRequestBydate(DateOnly keyword);
        Task<int> StartShipping(int? requestId);
    }
    public class ShippingRequestService : IShippingRequestService
    {
        private MyDbContext _context;
        private readonly IMapper _mapper;
        public ShippingRequestService(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Customer GetCustomerByUserId(int id)
        {
            var customer = _context.Customers.FirstOrDefault(x => x.UserId == id);
            if (customer == null)
            {
                return null;
            }
            return customer;

        }
        public List<WarehouseDtO> GetWarehouseByCutomerId(int customerId)
        {
            var warehouses = _context.Warehouses
                          .Where(x => x.CustomerId == customerId)
                          .Select(x => new WarehouseDtO
                          {
                              WarehouseId = x.WarehouseId,
                              Name = x.Name,
                              Address = x.Address,
                              Latitude = x.Latitude,
                              Longitude = x.Longitude,
                              Capacity = x.Capacity,
                              IsActive = x.IsActive,
                              ClosingTime = x.ClosingTime,
                              OpeningTime = x.OpeningTime
                          })
                          .ToList();
            return warehouses;
        }
        public List<ShippingRequetsDTO> GetShippingRByCutomerId(int customerId)
        {
            var shippingRequests = _context.ShippingRequests.Where(u => u.CustomerId == customerId).OrderByDescending(x => x.RequestDate)
                .Select(x => new ShippingRequetsDTO
                {
                    RequestId = x.RequestId,
                    RequestDate = x.RequestDate,
                    PickupLocation = x.PickupLocation,
                    DropoffLocation = x.DropoffLocation,
                    Weight = x.Weight,
                    ShippingCost = x.ShippingCost,
                    Note = x.Note,
                    Status = x.Status
                }).ToList();
            return shippingRequests;
        }
        public ShippingRequest getRequestById(int requestId)
        {
            var request = _context.ShippingRequests.SingleOrDefault( x => x.RequestId == requestId);
            if(request == null) {
                return null;
            }
            return request;
        }

        public void DeleteShippingById(int IdRequest)
        {
            var Request = _context.ShippingRequests.Where(x => x.RequestId == IdRequest).FirstOrDefault();
            if (Request != null)
            {
                Request.Status = "Đã hủy";
                _context.ShippingRequests.Update(Request);
                _context.SaveChanges();
            }
        }
        public void ConfirmReceivedById(int IdRequest)
        {
            var Request = _context.ShippingRequests.Where(x => x.RequestId == IdRequest).FirstOrDefault();
            if (Request != null)
            {
                Request.Status = "Đã nhận hàng";
                _context.ShippingRequests.Update(Request);
                _context.SaveChanges();
            }
        }
        public List<ProductTypeDto> GetAllProductTypes()
        {
            var productTypes = _context.ProductTypes
               .Select(pt => new ProductTypeDto
               {
                   ProductTypeId = pt.ProductTypeId,
                   Name = pt.Name
               })
               .ToList();
            return productTypes;
        }
        public void Create(ShippingRequetsDTO model, int userId)
        {
            var customer = _context.Customers.FirstOrDefault(x => x.UserId == userId);
            if (customer != null)
            {
                var request = _mapper.Map<ShippingRequest>(model);


                // Cập nhật các giá trị cho ShippingRequest
                request.RequestDate = DateOnly.FromDateTime(DateTime.Now);
                request.CustomerId = customer.CustomerId; // Gán CustomerId từ bảng Customer
                request.Status = "Chờ xác nhận";
                // Thêm vào cơ sở dữ liệu
                _context.ShippingRequests.Add(request);
                _context.SaveChanges();

            }

        }
        public List<ShippingRequest> getRequestBydate(DateOnly keyword)
        {
            var data = _context.ShippingRequests
                .Where(p => p.RequestDate == keyword)
                .ToList();
            if (data.Count > 0) {
                return data;
            }
            return null;
        }
        public async Task<int> StartShipping(int? requestId)
        {
            var requestt = await _context.ShippingRequests.FirstOrDefaultAsync(a => a.RequestId == requestId);
            if (requestt != null)
            {
                requestt.Status = "Đang giao hàng";
                _context.ShippingRequests.Update(requestt);
                _context.SaveChanges();
                return 1;
            }
            return -1;
        }


    }
}
