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
        public IActionResult AuthenOTP(string email)
        {
            ViewData["Email"] = email;
            return View();
        }
        public IActionResult ForgetPassword()
        {
            return View();
        }
        public IActionResult ResetPassword()
        {
            return View();
        }
    }
}
