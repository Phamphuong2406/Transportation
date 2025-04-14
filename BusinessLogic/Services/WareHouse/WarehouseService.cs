using AutoMapper;
using BusinessLogic.DTOs;
using DataAccess.DataContext;
using DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.WareHouse
{
    public class WarehouseService: IWarehouseService
    {
        private MyDbContext _context;
      //  private readonly IMapper _mapper;
        public WarehouseService(MyDbContext context) { 
        _context = context;
      
        }
        public Customer GetCustomerById(int IdUser)
        {
            var customer = _context.Customers.FirstOrDefault(x => x.UserId == IdUser);
            return customer;
        }
        public int Create(WarehouseDto warehouseDTo,int IdCustomer)

        {
            var warehouse = new Warehouse();
            warehouse.CustomerId = IdCustomer;
            warehouse.Name = warehouseDTo.Name;
            warehouse.Address = warehouseDTo.Address;
            warehouse.Latitude = warehouseDTo.Latitude.HasValue ? (decimal)warehouseDTo.Latitude.Value : null;
            warehouse.Longitude = warehouseDTo.Longitude.HasValue ? (decimal)warehouseDTo.Longitude.Value : null;
            warehouse.Capacity = warehouseDTo.Capacity;
            warehouse.IsActive = warehouseDTo.IsActive;

            warehouse.ClosingTime = TimeOnly.Parse(warehouseDTo.ClosingTime);
            warehouse.OpeningTime = TimeOnly.Parse(warehouseDTo.OpeningTime);
            
            _context.Warehouses.Add(warehouse);
            return _context.SaveChanges();
        }

    }
}
