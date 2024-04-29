using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BackendTest2.Models;

namespace BackendTest2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return this.View("~/Views/Mockups/Index.cshtml"); //Сделайте стартовой список задач или постмодерацию. 
        }

        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
