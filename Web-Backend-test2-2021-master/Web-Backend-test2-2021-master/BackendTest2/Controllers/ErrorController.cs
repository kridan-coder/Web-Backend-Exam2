using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BackendTest2.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index(String error)
        {
            ViewBag.Error = error;
            return View();
        }
    }
}
