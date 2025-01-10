using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Transportation.Infrastructure.Data;

namespace Transportation.ViewComponents
{

    public class OrderListViewComponent : ViewComponent
    {
        private readonly MyDbContext _context;

        public OrderListViewComponent(MyDbContext context)
        {
            _context = context;

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var oders = _context.ShippingRequests.ToList();
            return View(oders);

        }
    }
}