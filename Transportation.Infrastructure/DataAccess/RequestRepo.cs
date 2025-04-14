using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transportation.Domain.Interfaces;
using Transportation.Infrastructure.Data;

namespace Transportation.Infrastructure.DataAccess
{
    public class RequestRepo : IShippingRequestRepo
    {
        private MyDbContext _context;
        public RequestRepo(MyDbContext context)
        {
            _context = context;
        }
        // getAll
        public List<ShippingRequest> GetAll()
        {

            return _context.ShippingRequests.ToList();
        }
    }
}
