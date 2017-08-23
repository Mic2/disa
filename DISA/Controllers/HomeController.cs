﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DISA.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Movie()
        {
            ViewData["Message"] = "This is the movie reservation page!";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "This is the Contact page";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
