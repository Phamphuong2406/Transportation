﻿using Microsoft.AspNetCore.Mvc;

namespace Transportation.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }

        public IActionResult Register() {
            return View();
        }
    }
}
