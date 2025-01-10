using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Transportation.Application.DTO;
using Transportation.Application.Public;
using Transportation.Domain.ViewModel.Register;
using Transportation.Infrastructure.Data;

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
            var data = _context.Drivers.ToList();
            return View(data);
        }
      
        [HttpPost]
        public async Task<IActionResult> RegisterDriver(DriverDTO model)
        {

            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);

                user.RandomKey = Until.GenerateRandomkey();
                user.PasswordHash = model.PasswordHash.ToMd5Hash(user.RandomKey);
                user.RoleId = 3;
                user.IsActive = true;
                _context.Users.Add(user);
                _context.SaveChanges();
                //nếu user có role bằng 2 thì sẽ tạo mới 1 khách hàng 
                var driver = new Driver
                {

                    UserId = user.UserId,
                    FullName = model.FullName,
                    DateOfBirth = model.DateOfBirth,
                    Idcard = model.Idcard,
                    HealthStatus = model.HealthStatus
                };
                _context.Drivers.Add(driver);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");

            }

            return View();
        }
    }
    }

