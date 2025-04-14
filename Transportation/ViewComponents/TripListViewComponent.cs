using DataAccess.DataContext;
using Microsoft.AspNetCore.Mvc;

namespace Transportation.ViewComponents
{
    public class TripListViewComponent: ViewComponent
    {

        private readonly MyDbContext _context;

        public TripListViewComponent(MyDbContext context)
        {
            _context = context;

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var trips = _context.Trips.ToList();
            return View(trips);

        }
    }
}
