using DataAccess.DataContext;
using DataAccess.Entity;
using DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IUnitOfWork
    {
        IAssignmentRepo assignments { get; }
        IShippingRequestRepo shippingRequests { get; }
        ITripRepo trips { get; }
        Task<int> SaveAsync();
    }
    public class UnitOfWork:IUnitOfWork
    {
        private readonly MyDbContext _context;
        public IAssignmentRepo assignments { get; private set; }
        public IShippingRequestRepo shippingRequests { get; private set; }
        public ITripRepo trips { get; private set; }

        public UnitOfWork(MyDbContext context, IAssignmentRepo Assignments, IShippingRequestRepo ShippingRequests, ITripRepo Trips)
        {
            _context = context;
            assignments = Assignments;
            shippingRequests = ShippingRequests;
            trips = Trips;
        }

        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}

