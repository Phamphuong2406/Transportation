using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface IAccountRepo
    {
       
        int CreateCustomer(Customer customer);
        int CreateUser(Users user);
        int CreateDriver(Driver driver);

        int CreateDispatch(Dispatcher dispatcher);
    }
}
