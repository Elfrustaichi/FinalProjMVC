using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Voxo.Areas.Manage.ViewModels;
using Voxo.DAL;
using Voxo.Models;

namespace Voxo.Areas.Manage.Controllers
{
    [Area("manage")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly VoxoContext _context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager,VoxoContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
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
        //Login start
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
        //Login end

        //Logout start
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login");
        }
        //Logout end

        //Edit profile start
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Profile()
        {
            var loggedUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (loggedUser == null)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("login");
            }

            AdminProfileSettingViewModel viewModel = new AdminProfileSettingViewModel()
            {
                UserName=loggedUser.UserName,
                Email=loggedUser.Email,
                Phone=loggedUser.PhoneNumber,
                Fullname=loggedUser.Fullname,
            };

            return View(viewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult>  Profile(AdminProfileSettingViewModel viewModel)
        {
            if(!ModelState.IsValid)
            { 
                return View(viewModel);
            }

            AppUser loggedUser = await _userManager.FindByNameAsync(User.Identity.Name);

            loggedUser.Email = viewModel.Email;
            loggedUser.Fullname= viewModel.Fullname;
            loggedUser.UserName= viewModel.UserName;
            loggedUser.PhoneNumber = viewModel.Phone;

            var result = await _userManager.UpdateAsync(loggedUser);

            if(!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

                return View(viewModel);
            }
            await _signInManager.SignInAsync(loggedUser, false);

            return RedirectToAction("index","dashboard");
        }
        //Edit Profile end

        //Change password start
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangePassword()
        {
            var loggedUser=await _userManager.FindByNameAsync(User.Identity.Name);

            if (loggedUser == null)
            {
                return RedirectToAction("login");
            }
            AdminChangePasswordViewModel viewModel = new AdminChangePasswordViewModel();
            return View(viewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ChangePassword(AdminChangePasswordViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            var loggedUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (loggedUser == null)
            {
                return RedirectToAction("login");
            }

            loggedUser.PasswordHash = _userManager.PasswordHasher.HashPassword(loggedUser, viewModel.Password);
            var result = await _userManager.UpdateAsync(loggedUser);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                    
                }
                return View();
            }

            return RedirectToAction("index","dashboard");


        }
        //Change password end
    }
}
