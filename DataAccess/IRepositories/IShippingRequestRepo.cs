using DataAccess.DataContext;
using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace DataAccess.IRepositories
{
    public interface IShippingRequestRepo
    {
        List<ShippingRequest> GetAll();
        Task<ShippingRequest> GetByRequestId(int requestId);
        Task<int> UpdatePickupStatus(int requestId);
    }
    public class ShippingRequestRepo : IShippingRequestRepo
    {
        private readonly MyDbContext _context;
        public ShippingRequestRepo(MyDbContext context)
        {
            _context = context;
        }
        public List<ShippingRequest> GetAll()
        {

            return _context.ShippingRequests.ToList();
        }
        public async Task<ShippingRequest> GetByRequestId(int requestId)
        {

            return await _context.ShippingRequests
                         .FirstOrDefaultAsync(s => s.RequestId == requestId);
        }

        public async Task<int> UpdatePickupStatus(int requestId)
        {
            var request = await _context.ShippingRequests.FirstOrDefaultAsync(a => a.RequestId == requestId);
            if (request == null)
            {
                return -1;
            }
            request.Status = "Đang giao hàng";
            return 1;
        }

    }


}
