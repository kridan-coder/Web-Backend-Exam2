using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BackendTest2.Controllers
{
    public class MockupsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Users()
        {
            return View();
        }
        public IActionResult Tasks()
        {
            return View();
        }
        public IActionResult Topics()
        {
            return View();
        }
        public IActionResult CreateTask()
        {
            return View();
        }
        public IActionResult TaskDetail()
        {
            return View();
        }
        public IActionResult SendSolution()
        {
            return View();
        }
        public IActionResult Queue()
        {
            return View();
        }
        public IActionResult PostmoderationDetails()
        {
            return View();
        }
    }
}
