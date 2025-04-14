using DataAccess.DataContext;
using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccess.IRepositories
{
    public interface IShippingRequestRepo
    {
         List<ShippingRequest> GetAll();
        Task<ShippingRequest> GetByRequestId(int requestId);
    }
    public class ShippingRequestRepo : IShippingRequestRepo
    {
        private readonly MyDbContext _context;
        public  ShippingRequestRepo(MyDbContext context)
        {
            _context = context;
        }
        public List<ShippingRequest> GetAll()
        {

            return _context.ShippingRequests.ToList();
        }
        public async Task<ShippingRequest> GetByRequestId(int requestId) {

           return await _context.ShippingRequests
                        .FirstOrDefaultAsync(s => s.RequestId == requestId);
        }

    }

    
}
