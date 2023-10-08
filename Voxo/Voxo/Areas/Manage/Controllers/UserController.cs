using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Voxo.Areas.Manage.ViewModels;
using Voxo.DAL;
using Voxo.Models;
using Voxo.ViewModels;

namespace Voxo.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly VoxoContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UserController(VoxoContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        //User Index
        public IActionResult Index(int page=1, string search = null)
        {
            var query=_context.AppUsers.AsQueryable();

            if (search != null)
            {
                query = query.Where(x => x.UserName.Contains(search));
            }

            ViewBag.Search = search;

            return View(PaginatedList<AppUser>.Create(query,page,7));
        }
        //User Index

        //User Create start
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(AppUserCreateViewModel appUserVM)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            if(_userManager.Users.Any(x=>x.UserName==appUserVM.UserName))
            {
                ModelState.AddModelError("Username", "Username is already taken");
                return View();
            }

            if(_userManager.Users.Any(x=>x.Email==appUserVM.Email))
            {
                ModelState.AddModelError("Email","Email is already in use");
                return View();
            }
            AppUser newUser = new AppUser()
            {
                UserName = appUserVM.UserName,
                Email = appUserVM.Email,
                Fullname= appUserVM.Fullname,
                IsAdmin = appUserVM.IsAdmin,
                PhoneNumber=appUserVM.Phone,
                CreationTime=DateTime.Now,
            };
             
            var result=await _userManager.CreateAsync(newUser,appUserVM.Password);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                    return View();
                }
            }

            if(appUserVM.IsAdmin==true)
            {
                await _userManager.AddToRoleAsync(newUser, "Admin");
            }

            await _userManager.AddToRoleAsync(newUser, "Member");

            return RedirectToAction("index","user");
        }
        //User Create end

        //User delete start
        public IActionResult Delete(string id)
        {
            var User = _context.AppUsers.Find(id);

            if (User == null)
            {
                return StatusCode(404);
            }

            var userOrders=_context.Orders.Where(x=>x.AppUserId== User.Id).ToList();
            var userWishlist=_context.Wishlists.Where(x=>x.AppUserId== User.Id).ToList();
            var userCart=_context.UserCartItems.Where(x=>x.AppUserId== User.Id).ToList();
            var userReview=_context.ProductReviews.Where(x=>x.AppUserId==User.Id).ToList();

            _context.RemoveRange(userOrders);
            _context.RemoveRange(userWishlist);
            _context.RemoveRange(userCart);
            _context.RemoveRange(userReview);

            
            _context.AppUsers.Remove(User);
            _context.SaveChanges();

            return StatusCode(200);

        }
        //User delete end
        //User detail start
        public IActionResult Detail(string id)
        {
            var User =_context.AppUsers.Find(id);

            if (User==null)
            {
                return StatusCode(404);
            }
            
            return View(User);
        }
        //User detail end


    }
}
