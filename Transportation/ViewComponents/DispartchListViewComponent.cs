using DataAccess.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Transportation.ViewComponents
{
    public class DispartchListViewComponent : ViewComponent
    {
        private readonly MyDbContext _context;

        public DispartchListViewComponent(MyDbContext context)
        {
            _context = context;

        }
        public IViewComponentResult Invoke()
        {
            // Ánh xạ dữ liệu vào ViewModel
            var dispatch = _context.DispatchAssignments.Include(x =>x.Trip).ToList();

            return View(dispatch);
        }
    }
}
