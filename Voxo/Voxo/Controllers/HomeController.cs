using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Voxo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View("Error");
        }
    }
}