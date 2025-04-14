using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Public;
using DataAccess.DataContext;
using DataAccess.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Transportation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DriversController : Controller
    {
        private MyDbContext _context;
        private readonly IMapper _mapper;
        public DriversController(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
      
        [HttpPost]
        public IActionResult EditDriver(Driver model)
        {
            var driver = _context.Drivers.FirstOrDefault( x => x.DriverId == model.DriverId );
            if (driver == null)
            {
                return NotFound();
            }
            driver.FullName = model.FullName;
            driver.DateOfBirth = model.DateOfBirth;
            driver.Idcard = model.Idcard;
            driver.HealthStatus = model.HealthStatus;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult DeleteDriver(int DriverId)
        {
            var driver = _context.Drivers.FirstOrDefault(s => s.DriverId == DriverId);
            if (driver != null)
            {
                _context.Drivers.Remove(driver);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
    }

