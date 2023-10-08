using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voxo.DAL;
using Voxo.Models;
using Voxo.ViewModels;

namespace Voxo.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class SettingController : Controller
    {
        private readonly VoxoContext _context;

        public SettingController(VoxoContext context)
        {
            _context = context;
        }
        //Settings index start
        public IActionResult Index(int page=1, string search = null)
        {
            var query=_context.Settings.AsQueryable();

            if (search != null)
            {
                query = query.Where(x => x.Key.Contains(search));
            }

            ViewBag.Search = search;

            return View(PaginatedList<Setting>.Create(query,page,7));
        }
        //Settings index end
        //Setting create start
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Setting setting)
        {
            if (setting.Key == null)
            {
                ModelState.AddModelError("Key", "Must have key data");
                return View();
            }
            if(_context.Settings.Any(x=>x.Key==setting.Key))
            {
                ModelState.AddModelError("Key","Setting with this key already exists");
                return View();
            }
            if (setting.Value == null)
            {
                ModelState.AddModelError("Value", "Must have value data");
                return View();
            }

            _context.Settings.Add(setting);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
        //Setting create end

        //Setting delete start
        public IActionResult Delete(string key)
        {
            var existSetting = _context.Settings?.FirstOrDefault(x=>x.Key==key);

            if(existSetting==null)
            {
                return StatusCode(404);
            }

            _context.Settings.Remove(existSetting);
            _context.SaveChanges();

            return StatusCode(200);
        }
        //Setting delete end
        //Setting edit start
        public IActionResult Edit(string key)
        {
            var existSetting=_context.Settings?.FirstOrDefault(x => x.Key == key);

            if(existSetting == null)
            {
                return StatusCode(404);
            }

            return View(existSetting);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(Setting setting)
        {
            if (setting.Key == null)
            {
                ModelState.AddModelError("Key", "Must have key data");
                return View();
            }
            var existSetting=_context.Settings?.FirstOrDefault(x=>x.Key == setting.Key);
            if (existSetting == null)
            {
                return View("error");
            }
            
            if(existSetting.Key!=setting.Key&&_context.Settings.Any(x=>x.Key==setting.Key))
            {
                ModelState.AddModelError("Key", "Setting with this key already exists");
                return View(existSetting);
            }
            if (setting.Value == null)
            {
                ModelState.AddModelError("Value", "Must have value data");
                return View(existSetting);
            }

            existSetting.Key = setting.Key;
            existSetting.Value = setting.Value;

            _context.SaveChanges();
            return RedirectToAction("index");
        }
        //Setting edit end
    }
}
