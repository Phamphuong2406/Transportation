using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Transportation.Application.DTO;
using Transportation.Application.Public;

using Transportation.Infrastructure.Data;
using Transportation.ViewModel.Register;


namespace Transportation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DispatchersController : Controller
    {
        private MyDbContext _context;
        private readonly IMapper _mapper;
        public DispatchersController(MyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var model = _context.Dispatchers.ToList();
           

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterDispatcher([FromForm] DispatcherDTO model)
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
                var dispatcher = new Dispatcher
                {

                    UserId = user.UserId,
                    FullName = model.FullName,
                    Department = model.Department,
                };
                _context.Dispatchers.Add(dispatcher);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }

            return Ok();
        }
    }
}
