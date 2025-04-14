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
    public class DispatchersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

       
    }
}
