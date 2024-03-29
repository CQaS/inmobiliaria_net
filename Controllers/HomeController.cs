﻿using AplicacionPrueba.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionPrueba.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(ILogger<HomeController> logger)
        {            
            _logger = logger;
        }

        public IActionResult Index()
        {
          return View();
        }

        public IActionResult crear()
        {
            return View();
        }

        public IActionResult crear(Inquilino p)
        {            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public ActionResult Restringido()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
