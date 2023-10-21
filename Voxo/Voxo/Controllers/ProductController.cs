using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using Voxo.DAL;
using Voxo.Models;
using Voxo.ViewModels;

namespace Voxo.Controllers
{
    public class ProductController : Controller
    {
        private readonly VoxoContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public ProductController(VoxoContext context,UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        //Product review start
        [Authorize(Roles = "Member")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> PostComment(ProductReviewViewModel viewModel)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (viewModel.Review.ReviewText == null||viewModel.Review.Rate==0)
            {
                TempData["Error"] = "Comment is wrong";
                return RedirectToAction("index", "home");
            }

            if (user == null)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("login","account");
            }

            var product=_context.Products.Include(x=>x.ProductReviews).FirstOrDefault(x=>x.Id==viewModel.ProductId);

            if (product == null)
            {
                return View("error");
            }

            ProductReview review= new ProductReview();

            review.ProductId=viewModel.ProductId;
            review.AppUserId=user.Id;
            review.CreateTime=DateTime.Now;
            review.Rate = viewModel.Review.Rate;
            review.ReviewText=viewModel.Review.ReviewText;
            review.IsPublised = false;

            product.Rate=Convert.ToInt32(Math.Ceiling( d:(product.ProductReviews.Sum(x=>x.Rate)/product.ProductReviews.Count())));

            product.ProductReviews.Add(review);
            _context.SaveChanges();

            return RedirectToAction("index","home");
        }
        
        //Product review end
        //Product detail start
        public IActionResult Detail(int id)
        {
            Product product=_context.Products
                .Include(x=>x.ProductImages)
                .Include(x => x.ProductTags).ThenInclude(x=>x.Tag)
                .Include(x=>x.Category)
                .Include(x=>x.Brand)
                .Include(x => x.ProductReviews)
                .Include(x=>x.ProductSizes).ThenInclude(x=>x.Size)
                .FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return View("Error");
            }

            ProductDetailViewModel viewModel = new ProductDetailViewModel
            {
                Product = product,
                RelatedProducts = _context.Products.Where(x => x.CategoryId == product.CategoryId).Include(x => x.ProductImages).Include(x => x.ProductTags).ThenInclude(x => x.Tag).Include(x=>x.Brand).ToList(),
                ProductReview = new ProductReview { ProductId = product.Id },
                Reviews = _context.ProductReviews.Include(x => x.Product).ThenInclude(x => x.ProductImages).Include(x=>x.AppUser).Where(x => x.ProductId == id).ToList()
                

            };
            return View(viewModel);
        }
        //Product detail start

        //Product quick-view modal start
        public IActionResult ProductDetailModal(int id)
        {
            Product product=_context.Products
                .Include(x=>x.ProductImages)
                .Include(x=>x.ProductSizes).ThenInclude(x=>x.Size)
                .Include(x=>x.ProductTags).ThenInclude(x=>x.Tag)
                .Include(x=>x.Brand)
                .Include(x=>x.Category)
                .FirstOrDefault(x=>x.Id==id);

            if(product==null) { return StatusCode(404); }

            return PartialView("_ProductModalPartial",product);
        }
        //Product quick-view modal end

        //Product cart start
        
        public IActionResult AddToCart(int id)
        {
            if (User.Identity.IsAuthenticated )
            {

                string userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
                var cartItem=_context.UserCartItems.FirstOrDefault(x=>x.ProductId==id&&x.AppUserId==userId);

                if(cartItem!=null)
                {
                    cartItem.Count++;
                }
                else
                {
                    UserCartItem newCartItem = new UserCartItem
                    {
                        AppUserId=userId,
                        ProductId=id,
                        Count=1
                    };
                    _context.UserCartItems.Add(newCartItem);
                }
                _context.SaveChanges();
                var cartItemsDropdown=_context.UserCartItems.Include(x=>x.Product).ThenInclude(x=>x.ProductImages).Where(x=>x.AppUserId==userId).ToList();

                return PartialView("_CartPartialView",GenerateCart(cartItemsDropdown));
            }
            else
            {
                List<CartItemCookieViewModel> cartItems = new List<CartItemCookieViewModel>();

                CartItemCookieViewModel cartItem;
                var cartCookie = Request.Cookies["cart"];
                if (cartCookie != null)
                {
                    cartItems = JsonConvert.DeserializeObject<List<CartItemCookieViewModel>>(cartCookie);

                    cartItem = cartItems.FirstOrDefault(x => x.ProductId == id);
                    if (cartItem != null)
                    {
                        cartItem.Count++;
                    }
                    else
                    {
                        cartItem = new CartItemCookieViewModel { ProductId = id, Count = 1 };

                        cartItems.Add(cartItem);
                    }
                }
                else
                {
                    cartItem = new CartItemCookieViewModel { ProductId = id, Count = 1 };
                    cartItems.Add(cartItem);
                }
                
                Response.Cookies.Append("Cart", JsonConvert.SerializeObject(cartItems));

                return PartialView("_CartPartialView",GenerateCart(cartItems));
            }
        }

        private CartViewModel GenerateCart(List<UserCartItem> cartItems)
        {
            CartViewModel viewModel = new CartViewModel();

            foreach (var item in cartItems)
            {
                CartItemViewModel cartItemViewModel = new CartItemViewModel
                {
                    Product=item.Product,
                    Count=item.Count,
                };
                viewModel.Items.Add(cartItemViewModel);
                viewModel.TotalPrice += (item.Product.DiscountPercent > 0 ? ((item.Product.SalePrice * (item.Product.SalePrice * item.Product.DiscountPercent / 100))): item.Product.SalePrice) * item.Count;
            }

            return viewModel;
            
        }
        private CartViewModel GenerateCart(List<CartItemCookieViewModel> cartItems)
        {
            CartViewModel viewModel= new CartViewModel();

            foreach (var item in cartItems)
            {
                CartItemViewModel cartViewModel = new CartItemViewModel
                {
                    Product = _context.Products.Include(x=>x.ProductImages).FirstOrDefault(x => x.Id == item.ProductId),
                    Count=item.Count,
                };

                viewModel.Items.Add(cartViewModel);
                viewModel.TotalPrice += (cartViewModel.Product.DiscountPercent > 0 ? ((cartViewModel.Product.SalePrice * (cartViewModel.Product.SalePrice * cartViewModel.Product.DiscountPercent / 100))) : cartViewModel.Product.SalePrice) * item.Count;
            }

            return viewModel;
        }
        public IActionResult Cart()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var cartItems = _context.UserCartItems.Include(x => x.Product).ThenInclude(x => x.ProductImages).Where(x => x.AppUserId == userId).ToList();
                if (!cartItems.Any())
                {
                    TempData["Error"] = "There is no item in cart";
                    return RedirectToAction("index", "home");
                }
                return View(GenerateCart(cartItems));

            }
            else
            {
                var cookieItems = Request.Cookies["Cart"];
                
                var cartItems = JsonConvert.DeserializeObject<List<CartItemCookieViewModel>>(cookieItems);
                if (!cartItems.Any())
                {
                    TempData["Error"] = "There is no item in cart";
                    return RedirectToAction("index", "home");
                }
                return View(GenerateCart(cartItems));
            }
            
        }
        //Product cart end
        public IActionResult RemoveCart(int id)
        {
            if (User.Identity.IsAuthenticated)
            {

                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var cartItem = _context.UserCartItems.FirstOrDefault(x => x.ProductId == id && x.AppUserId == userId);

                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                }
                else
                {
                    _context.UserCartItems.Remove(cartItem);
                }
                _context.SaveChanges();
                var cartItemsDropdown = _context.UserCartItems.Include(x => x.Product).ThenInclude(x => x.ProductImages).Where(x => x.AppUserId == userId).ToList();

                if (!_context.UserCartItems.Where(x=>x.AppUserId==userId).Any())
                {
                    TempData["Success"] = "Item deleted succesfully";
                    return RedirectToAction("index", "home");
                }
                TempData["Success"] = "Item deleted succesfully";
                return RedirectToAction("cart");
            }
            var basketStr = Request.Cookies["Cart"];
            if (basketStr == null)
                return StatusCode(404);

            List<CartItemCookieViewModel> cookieItems = JsonConvert.DeserializeObject<List<CartItemCookieViewModel>>(basketStr);

            CartItemCookieViewModel item = cookieItems.FirstOrDefault(x => x.ProductId == id);

            if (item == null)
                return StatusCode(404);

            if (item.Count > 1)
                item.Count--;
            else
                cookieItems.Remove(item);

            Response.Cookies.Append("Cart", JsonConvert.SerializeObject(cookieItems));

            CartViewModel bv = new CartViewModel();
            foreach (var ci in cookieItems)
            {
                CartItemViewModel bi = new CartItemViewModel
                {
                     Count= ci.Count,
                    Product = _context.Products.Include(x => x.ProductImages).FirstOrDefault(x => x.Id == ci.ProductId)
                };
                bv.Items.Add(bi);
                bv.TotalPrice += (bi.Product.DiscountPercent > 0 ? (bi.Product.SalePrice * (100 - bi.Product.DiscountPercent) / 100) : bi.Product.SalePrice) * bi.Count;
            }
            if (!cookieItems.Any())
            {
                TempData["Success"] = "Item deleted succesfully";
                return RedirectToAction("index", "home");
            }
            TempData["Success"] = "Item deleted succesfully";
            return RedirectToAction("cart");
        }

        public IActionResult WishList()
        {
            return View();
        }
    }
}
