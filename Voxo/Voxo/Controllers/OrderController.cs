using Microsoft.AspNetCore.Mvc;

namespace Voxo.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Checkout()
        {
            return View();
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }
    }
}
