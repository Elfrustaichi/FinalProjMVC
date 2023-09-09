using Microsoft.AspNetCore.Mvc;

namespace Voxo.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Forgot()
        {
            return View();
        }
    }
}
