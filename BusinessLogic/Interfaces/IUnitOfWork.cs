using DataAccess.DataContext;
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
        IAssignmentRepo Assignments { get; }
        IShippingRequestRepo ShippingRequests { get; }
        ITripRepo Trips { get; }
        Task<int> SaveAsync();
    }
    public class UnitOfWork:IUnitOfWork
    {
        private readonly MyDbContext _context;
        public IAssignmentRepo Assignments { get; private set; }
        public IShippingRequestRepo ShippingRequests { get; private set; }
        public ITripRepo Trips { get; private set; }

        public UnitOfWork(MyDbContext context)
        {
            _context = context;
            Assignments = new AssignmentRepo(_context);
            ShippingRequests = new ShippingRequestRepo(_context);
            Trips = new TripRepo(_context);
        }

        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}

