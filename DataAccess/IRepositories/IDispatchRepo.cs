using Azure.Core;
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
    public interface IAssignmentRepo
    {
        Task<DispatchAssignment> GetByRequestId(int RequestId);
        Task<int> UpdatePickupStatus(int requestId);
    }
    public class AssignmentRepo : IAssignmentRepo
    {
        private MyDbContext _context;
        public AssignmentRepo(MyDbContext context)
        {
            _context = context;
        }
        public async Task<DispatchAssignment> GetByRequestId(int RequestId)
        {
            return await _context.DispatchAssignments.FirstOrDefaultAsync(a => a.RequestId == RequestId);
        }
        public async Task<int> UpdatePickupStatus(int requestId) 
        {
            var assignment = await _context.DispatchAssignments.FirstOrDefaultAsync(a => a.RequestId == requestId);
            if (assignment == null) {
                return -1;
            }
            assignment.Status = "Đã lấy hàng";
            assignment.Pickupdate = DateTime.Now;
            return 1;
        }
    }
}
