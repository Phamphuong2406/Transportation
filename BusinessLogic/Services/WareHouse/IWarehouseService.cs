using BusinessLogic.DTOs;
using DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.WareHouse
{
    public interface IWarehouseService
    {
        Customer GetCustomerById(int IdUser);
        int Create(WarehouseDto warehouseDTo, int IdCustomer);
    }
}
