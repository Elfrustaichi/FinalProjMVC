using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voxo.DAL;
using Voxo.Helpers;
using Voxo.Models;
using Voxo.ViewModels;

namespace Voxo.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class BannerController : Controller
    {
        private readonly VoxoContext _context;
        private readonly IWebHostEnvironment _env;

        public BannerController(VoxoContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        //Banner index start
        public IActionResult Index(int page=1,string search=null)
        {
            var query=_context.Banners.AsQueryable();

            if (search != null)
            {
                query = query.Where(x => x.Header.Contains(search));
                
            }
            ViewBag.Search = search;


            return View(PaginatedList<Banner>.Create(query,page,7));
        }
        //Banner index end

        //Banner create start
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Banner banner)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            if(_context.Banners.Any(x=>x.Header==banner.Header))
            {
                ModelState.AddModelError("Header","Banner already exists");
                return View();
            }
            if(banner.BackgroundImage==null)
            {
                ModelState.AddModelError("BackgroundImage", "Banner must have background image");
                return View();
            }
            if (banner.Type == "ShopBanner")
            {
                if (_context.Banners.Any(x=>x.Type=="ShopBanner"))
                {
                    ModelState.AddModelError("", "You can only have 1 shop banner");
                    return View();
                }
            }
            if (banner.Type == "DealOfTheDayBanner")
            {
                if (_context.Banners.Any(x => x.Type == "DealOfTheDayBanner"))
                {
                    ModelState.AddModelError("", "You can only have 1 DealOfTheDayBanner");
                    return View();
                }
            }

            banner.BackgroundImageName = FileManager.Save(_env.WebRootPath,"uploads/BannerImages",banner.BackgroundImage);

            _context.Banners.Add(banner);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
        //Banner create end
        //Banner delete start
        public IActionResult Delete(int id)
        {
            var existBanner = _context.Banners.Find(id);
            if (existBanner == null)
            {
                return StatusCode(404);
            }

            string oldImage = null;
            oldImage = existBanner.BackgroundImageName;

            _context.Banners.Remove(existBanner);
            _context.SaveChanges();
            if (oldImage != null)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/BannerImages", oldImage);
            }

            return StatusCode(200);

        }
        //Banner delete end

        //Banner edit start
        public IActionResult Edit(int id)
        {
            var existBanner = _context.Banners.Find(id);
            if (existBanner == null)
            {
                return StatusCode(404);
            }

            return View(existBanner);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Banner banner)
        {
            var existBanner= _context.Banners.Find(banner.Id);
            if (existBanner == null)
            {
                return View("Error");
            }
            if(!ModelState.IsValid)
            {
                return View(existBanner);
            }

            if (existBanner.Header != banner.Header && _context.Banners.Any(x => x.Header == banner.Header))
            {
                ModelState.AddModelError("Header", "Banner already exists");
                return View(existBanner);
            }
            existBanner.Header = banner.Header;
            existBanner.Offer = banner.Offer;
            existBanner.OfferTime = banner.OfferTime;
            existBanner.ButtonText = banner.ButtonText;
            existBanner.ButtonUrl = banner.ButtonUrl;
            existBanner.DiscountText = banner.DiscountText;
            existBanner.Description = banner.Description;
            existBanner.UltraBannerSecondHeader = banner.UltraBannerSecondHeader;
            existBanner.Price = banner.Price;
            existBanner.Size = banner.Size;

            string oldImage = null;
            if(banner.BackgroundImage != null)
            {
                oldImage = existBanner.BackgroundImageName;
                existBanner.BackgroundImageName=FileManager.Save(_env.WebRootPath,"uploads/BannerImages",banner.BackgroundImage);
            }
            _context.SaveChanges();

            if (oldImage != null)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/BannerImages", oldImage);
            }

            return RedirectToAction("index");
        }
        //Banner edit end
        //Banner detail start
        public IActionResult Detail(int id)
        {
            var existBanner = _context.Banners.Find(id);
            if(existBanner == null)
            {
                return StatusCode(404);
            }
            return View(existBanner);
        }
        //Banner detail end
    }
}
