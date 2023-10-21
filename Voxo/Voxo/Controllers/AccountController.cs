using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Voxo.DAL;
using Voxo.Models;
using Voxo.Services;
using Voxo.ViewModels;

namespace Voxo.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly VoxoContext _context;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,VoxoContext context,IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _context = context;
        }
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Dashboard()
        {
            AppUser user= _context.AppUsers
                .Include(x=>x.Orders).ThenInclude(x=>x.OrderItems)
                .Include(x=>x.CartItems)
                .FirstOrDefault(x=>x.UserName==User.Identity.Name);
            if (user == null)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("login");
            }

            return View(user);
        }
        [Authorize(Roles = "Member")]
        public IActionResult OrderDetail(int orderId,string username)
        {
            AppUser user=_context.AppUsers.FirstOrDefault(x=>x.UserName==username);
            if (user==null)
            {
                TempData["Error"] = "user not found";
                return RedirectToAction("index","home");
            }
            if (user.Orders.Any(x => x.Id == orderId))
            {
                TempData["Error"] = "order not found";
                return RedirectToAction("index", "home");
            }
            var order=_context.Orders.Include(x=>x.OrderItems).ThenInclude(x=>x.Product).ThenInclude(x=>x.ProductImages).FirstOrDefault(x=>x.Id==orderId&&x.AppUser==user);
            return View(order);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(MemberLoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser user= await _userManager.FindByNameAsync(viewModel.UserName);

            if(user == null)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user,viewModel.Password,false,false);

            if(!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();
            }

            return RedirectToAction("Index","home");

        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(MemberRegisterViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            if(_userManager.Users.Any(x=>x.UserName==viewModel.Username))
            {
                ModelState.AddModelError("Username", "Username is already taken");
                return View();
            }
            if(_userManager.Users.Any(x=>x.Email==viewModel.Email))
            {
                ModelState.AddModelError("Email", "Email is already in use");
                return View();
            }

            AppUser user = new AppUser
            {
                UserName = viewModel.Username,
                Email = viewModel.Email,
                CreationTime=DateTime.Now,
                IsAdmin=false
            };

            var result=await _userManager.CreateAsync(user,viewModel.Password);

            if(!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                    return View();
                }
            }

            await _userManager.AddToRoleAsync(user, "Member");

            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Index","home");

        }
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["Success"] = "logged out";
            
            return RedirectToAction("index", "home");
        }
        public IActionResult Forgot()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Forgot(ForgotViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            var user =await _userManager.FindByEmailAsync(viewModel.Email);

            if (user == null)
            {
                ModelState.AddModelError("Email", "Email is not correct");
                return View();
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            string url = Url.Action("resetpassword","account",new {email=viewModel.Email,token=token},Request.Scheme);

            _emailSender.Send(viewModel.Email,"Reset Password", $"Click <a href=\"{url}\">here</a> to reset your password");

            TempData["Success"] = "Reset email sent";
            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> ResetPassword(string email,string token)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);

            if(user == null||!await _userManager.VerifyUserTokenAsync(user,_userManager.Options.Tokens.PasswordResetTokenProvider,"ResetPassword",token))
            {
                return RedirectToAction("login","account");
            }

            ViewBag.Email = email;  
            ViewBag.Token = token;

            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user=await _userManager.FindByEmailAsync(viewModel.Email);

            if (user == null)
            {
                TempData["Error"] = "user not found";
                return RedirectToAction("index", "home");
            }

            var result= await _userManager.ResetPasswordAsync(user, viewModel.Token,viewModel.Password);

            if (!result.Succeeded)
            {
                TempData["Error"] = "user not found";
                return RedirectToAction("index", "home");
            }

            TempData["Success"] = "Password resetted";
            return RedirectToAction("index", "home");
        }
        [Authorize(Roles ="Member")]
        public async Task<IActionResult> EditProfile()
        {
            var user= await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                TempData["Error"] = "user not found";
                return RedirectToAction("index", "home");
            }
            ProfileEditViewModel viewModel = new ProfileEditViewModel
            {
                Address = user.Adress,
                Email = user.Email,
                Username = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Fullname=user.Fullname,
            };

            return PartialView("_ProfileEditPartialView",viewModel);
        }
        [Authorize(Roles = "Member")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditProfile(ProfileEditViewModel viewModel)
        {
            var user= await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("login");
            }
            if(viewModel.Username !=user.UserName&&_context.AppUsers.Any(x=>x.UserName==viewModel.Username)) 
            {
                TempData["Error"] = "Username is already in use";
                return RedirectToAction("dashboard");
            }

            if(viewModel.Username != null)
            {
                user.UserName = viewModel.Username;
            }
            if (viewModel.Email != null)
            {
                user.Email = viewModel.Email;
            }
            if (viewModel.Address != null)
            {
                user.Adress = viewModel.Address;
            }
            if (viewModel.PhoneNumber != null)
            {
                user.PhoneNumber = viewModel.PhoneNumber;
            }
            if (viewModel.Fullname != null)
            {
                user.Fullname = viewModel.Fullname;
            }

            var result= await _userManager.UpdateAsync(user);

            if(!result.Succeeded)
            {
                TempData["Error"] = "User not updated";
                return RedirectToAction("dashboard");
            }
            TempData["Success"] = "user edited";
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("dashboard");
        }
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> ChangePassword()
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name);
            if(user== null)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("login");
            }

            ChangePasswordViewModel changeViewModel = new ChangePasswordViewModel();

            return PartialView("_ChangePasswordPartialView",changeViewModel);
        }
        [Authorize(Roles = "Member")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                TempData["Error"] = "something went wrong";
                return RedirectToAction("dashboard");
            }
            var loggedUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (loggedUser == null)
            {
                TempData["Error"] = "user not found";
                return RedirectToAction("login", "account");
            }

            loggedUser.PasswordHash = _userManager.PasswordHasher.HashPassword(loggedUser, viewModel.Password);
            var result = await _userManager.UpdateAsync(loggedUser);
            if (!result.Succeeded)
            {
                TempData["Error"] = "Something went wrong";
                return RedirectToAction("dashboard");
            }

            return RedirectToAction("dashboard");

        }

        
    }
}
