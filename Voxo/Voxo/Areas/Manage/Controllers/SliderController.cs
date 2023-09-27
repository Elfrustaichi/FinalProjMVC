using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Voxo.DAL;
using Voxo.Helpers;
using Voxo.Models;
using Voxo.ViewModels;

namespace Voxo.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class SliderController : Controller
    {
        private readonly VoxoContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(VoxoContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        //Slider index start
        public IActionResult Index(int page=1)
        {
            var query=_context.Sliders.Include(x=>x.Product).AsQueryable();

            return View(PaginatedList<Slider>.Create(query,page,7));
        }
        //Slider index end
        //Slider create start
        public IActionResult Create()
        {
            GetProducts();

            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Slider slider)
        {
            GetProducts();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (slider.SliderImage == null)
            {
                ModelState.AddModelError("SliderImage","Slider must have image");
                return View();
            }
            if (_context.Sliders.Any(x=>x.TitleText==slider.TitleText))
            {
                ModelState.AddModelError("TitleText", "Slider already exist");
                return View();
            }
            if (!_context.Products.Any(x => x.Id == slider.ProductId))
            {
                ModelState.AddModelError("ProductId", "Product not found");
                return View();
            }
            var existProductId = _context.Sliders.FirstOrDefault(x => x.ProductId == slider.ProductId);
            if (existProductId != null)
            {
                ModelState.AddModelError("ProductId", "Product have been used in another slider");
                return View();
            }

            slider.BackgroundImageName = FileManager.Save(_env.WebRootPath, "uploads/SliderImages", slider.SliderImage);

            _context.Sliders.Add(slider);
            _context.SaveChanges();

            return RedirectToAction("index");

        }
        //Slider create end
        //Slider delete start
        public IActionResult Delete(int id)
        {
            var existSlider = _context.Sliders.Find(id);

            if(existSlider == null)
            {
                return StatusCode(404);
            }

            if (existSlider.BackgroundImageName != null)
            {
                FileManager.Delete(_env.WebRootPath,"uploads/SliderImages",existSlider.BackgroundImageName);
            }

            _context.Sliders.Remove(existSlider);
            _context.SaveChanges();

            return StatusCode(200);
        }
        //Slider delete end

        //Slider edit start
        public IActionResult Edit(int id)
        {
            GetProducts();
            var existSlider=_context.Sliders.Include(x=>x.Product).FirstOrDefault(x=>x.Id==id);
            if(existSlider == null)
            {
                return StatusCode(404);
            }

            return View(existSlider);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Slider slider)
        {
            GetProducts();
            var exitsSlider=_context.Sliders.Include(x=>x.Product).FirstOrDefault(x=>x.Id==slider.Id);

            if(exitsSlider == null)
            {
                return View("error");
            }
            if(!ModelState.IsValid)
            {
                return View(exitsSlider);
            }
            if (exitsSlider.TitleText != slider.TitleText && _context.Sliders.Any(x => x.TitleText == slider.TitleText))
            {
                ModelState.AddModelError("TitleText", "Slider already exist");
            }
            var existProductId = _context.Sliders.FirstOrDefault(x => x.ProductId == slider.ProductId);
            if (existProductId != null)
            {
                ModelState.AddModelError("ProductId", "Product have been used in another slider");
                return View();
            }

            exitsSlider.TitleText = slider.TitleText;
            exitsSlider.SaleText = slider.SaleText;
            exitsSlider.OfferText = slider.OfferText;
            exitsSlider.ButtonText = slider.ButtonText;
            exitsSlider.ButtonUrl = slider.ButtonUrl;
            exitsSlider.ProductId = slider.ProductId;
            exitsSlider.ButtonHeaderText = slider.ButtonHeaderText;

            string oldImage = null;
            if(slider.SliderImage != null)
            {
                oldImage = exitsSlider.BackgroundImageName;
                exitsSlider.BackgroundImageName = FileManager.Save(_env.WebRootPath,"uploads/SliderImages",slider.SliderImage);
            }

            _context.SaveChanges();

            if(oldImage!= null)
            {
                FileManager.Delete(_env.WebRootPath,"uploads/SliderImages",oldImage);
            }

            return RedirectToAction("index");
        }
        //Slider edit end
        //Slider detail start
        public IActionResult Detail(int id)
        {
            var exitsSlider=_context.Sliders.Include(x=>x.Product).FirstOrDefault(x=>x.Id==id);
            if (exitsSlider == null)
            {
                return StatusCode(404);
            }

            return View(exitsSlider);
        }
        //Slider detail end

        public void GetProducts()
        {
            ViewBag.Products = _context.Products.Where(x=>x.Slider==null);
        }

    }
}
