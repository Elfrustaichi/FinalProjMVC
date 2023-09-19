using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using Voxo.DAL;
using Voxo.Helpers;
using Voxo.Models;
using Voxo.ViewModels;

namespace Voxo.Areas.Manage.Controllers
{
    [Authorize(Roles ="Admin")]
    [Area("manage")]
    public class CategoryController : Controller
    {
        private readonly VoxoContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryController(VoxoContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        //Category index start
        public IActionResult Index(int page=1)
        {
            var query=_context.Categories.AsQueryable();
            
            return View(PaginatedList<Category>.Create(query,page,7));
        }
        //Category index end

        //Category create start
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Category category)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            if (_context.Categories.Any(x => x.Name == category.Name))
            {
                ModelState.AddModelError("Name", "Category already exists");
                return View();
            }
            if (category.CategoryImage != null)
            {
                category.BackgroundImageName = FileManager.Save(_env.WebRootPath, "uploads/CategoryImages", category.CategoryImage);
            }
            

            _context.Categories.Add(category);
            _context.SaveChanges();

            return RedirectToAction("index","category");
        }
        //Category create end

        //Category delete start
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);

            if (category == null)
            {
                return StatusCode(404);
            }

            string oldImage=category.BackgroundImageName;

            _context.Categories.Remove(category);
            _context.SaveChanges();
            if (oldImage != null)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/CategoryImages", oldImage);
            }
            
            return StatusCode(200);
        }
        //Category delete end

        //Category edit start
        public IActionResult Edit(int id)
        {
            var category= _context.Categories.Find(id);
            if (category == null)
            {
                return StatusCode(404);
            }
            return View(category);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Category category)
        {
           
            var existCategory=_context.Categories.Find(category.Id);

            if (!ModelState.IsValid)
            {
                return View(existCategory);
            }
            if (existCategory==null)
            {
                return View("Error");
            }

            if(category.Name!=existCategory.Name&&_context.Categories.Any(x=>x.Name==category.Name))
            {
                ModelState.AddModelError("Name","Category already exist");
                return View(existCategory);
            }

            existCategory.Name= category.Name;
            existCategory.CategoryTag= category.CategoryTag;

            string oldImageName = null;
            if(category.CategoryImage!=null)
            {
                oldImageName = existCategory.BackgroundImageName;
                existCategory.BackgroundImageName = FileManager.Save(_env.WebRootPath, "uploads/CategoryImages", category.CategoryImage);
            }

            _context.SaveChanges();

            if(oldImageName!=null)
            {
                FileManager.Delete(_env.WebRootPath,"uploads/CategoryImages",oldImageName);
            }

            return RedirectToAction("index","category");
        }
        //Category edit end
        
    }
}
