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
    public interface ITripRepo
    {
        Task<Trip> GetByTripId(int tripId);
    }
    public class TripRepo : ITripRepo
    {
        private MyDbContext _context;
        public TripRepo(MyDbContext context)
        {
            _context = context;
        }
        public async Task<Trip> GetByTripId(int tripId)
        {
            return await _context.Trips.FirstOrDefaultAsync(x => x.TripId == tripId);

        }
        public async Task<int> UpdatePickupStatus(int requestId)
        {
            var assignment = await _context.ShippingRequests.FirstOrDefaultAsync(a => a.RequestId == requestId);
            if (assignment == null)
            {
                return -1;
            }
            assignment.Status = "Đã lấy hàng";
            assignment.Pickupdate = DateTime.Now;
            return 1;
        }
    }
}
