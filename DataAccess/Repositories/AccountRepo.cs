using DataAccess.DataContext;
using DataAccess.Entity;
using DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class AccountRepo: IAccountRepo
    {
        private readonly MyDbContext _context;

        public AccountRepo(MyDbContext context)
        {
            _context = context;
        }
        public int CreateCustomer(Customer customer)
        {
            try
            {
                _context.Customers.Add(customer);
                return _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }

            
        }
        public int CreateUser(Users user)
        {
            try
            {
                _context.Users.Add(user);
                return _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }


        }
        public int CreateDriver(Driver driver)
        {
            try
            {
                _context.Drivers.Add(driver);
                return _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }


        }
        public int CreateDispatch(Dispatcher dispatcher)
        {
            try
            {
                _context.Dispatchers.Add(dispatcher);
                return _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }


        }

    }
}
