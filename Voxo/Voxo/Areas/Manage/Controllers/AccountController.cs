using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Voxo.Areas.Manage.ViewModels;
using Voxo.Models;

namespace Voxo.Areas.Manage.Controllers
{
    [Area("manage")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        //public async Task<IActionResult> CreateAdmin()
        //{
        //    AppUser admin = new AppUser()
        //    {
        //        UserName = "admin",
        //        IsAdmin = true,
        //        Email="umid.museyibli.9@gmail.com"
        //    };

        //    var result= await _userManager.CreateAsync(admin,"admin1245");

        //    await _userManager.AddToRoleAsync(admin, "Admin");
        //    return Json(result);
        //}
        //public async Task<IActionResult> CreateRoles()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole("Admin"));
        //    await _roleManager.CreateAsync(new IdentityRole("Member"));
        //    return Ok();
        //}
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(AdminLoginViewModel adminVM)
        {
            AppUser user = await _userManager.FindByNameAsync(adminVM.Username);

            if (user == null)
            {
                ModelState.AddModelError("","Username or password is incorrect");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(user, adminVM.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password is incorrect ");
                return View();
            }

            return RedirectToAction("index","dashboard");
        }
    }
}
