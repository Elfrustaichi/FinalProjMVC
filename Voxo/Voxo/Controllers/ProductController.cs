using Microsoft.AspNetCore.Mvc;

namespace Voxo.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Detail()
        {
            return View();
        }

        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult WishList()
        {
            return View();
        }
    }
}
