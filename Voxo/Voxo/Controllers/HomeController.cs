using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Voxo.DAL;
using Voxo.Models;
using Voxo.ViewModels;

namespace Voxo.Controllers
{
    public class HomeController : Controller
    {
        private readonly VoxoContext _context;

        public HomeController(VoxoContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeViewModel viewModel = new HomeViewModel();

            viewModel.Sliders=_context.Sliders
                .Include(x=>x.Product).ThenInclude(x=>x.ProductReviews)
                .Include(x=>x.Product).ThenInclude(x=>x.ProductSizes).ThenInclude(x=>x.Size)
                .ToList();
            viewModel.NewArrivalBanners=_context.Banners.Where(x=>x.Type=="NewArrivalBanner").ToList();
            viewModel.NewArrivalProducts=_context.Products
                .Include(x=>x.ProductImages)
                .Include(x=>x.Category)
                .Include(x=>x.Brand)
                .Include(x => x.ProductTags).ThenInclude(x => x.Tag)
                .Where(x=>x.IsNewArrival==true&&x.Slider==null).ToList();
            viewModel.NewOfferBanners = _context.Banners.Where(x => x.Type == "NewOfferBanner").ToList();
            viewModel.LatestProducts=_context.Products
                .Include(x=>x.ProductImages)
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .Where(x=>x.IsNewArrival==false && x.Slider == null).ToList();
            viewModel.InstagramBanners = _context.Banners.Where(x => x.Type == "InstagramBanner").ToList();
            viewModel.DealOfTheDayBanner = _context.Banners.FirstOrDefault(x=>x.Type=="DealOfTheDayBanner");
            viewModel.HomeServices = _context.Services.Where(x => x.Type == "Home").ToList();
            viewModel.TopCategories = _context.Categories.Where(x => x.CategoryTag == "New").ToList();

            return View(viewModel);
        }

        public IActionResult FAQ()
        {
            FaqViewModel viewModel = new FaqViewModel();

            viewModel.FaqServices=_context.Services.Where(x=>x.Type=="faq").ToList();
            viewModel.Faqs = _context.FAQs.ToList();

            return View(viewModel);
        }

        public IActionResult ContactUs()
        {
            ViewBag.Settings=_context.Settings.ToDictionary(x=>x.Key,x=>x.Value);
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult ContactUs(ContactUsViewModel viewModel)
        {
            ViewBag.Settings = _context.Settings.ToDictionary(x => x.Key, x => x.Value);

            if(!ModelState.IsValid)
            {
                return View(viewModel);
            }

            ContactUsRequest request = new ContactUsRequest
            {
                Name= viewModel.Name,
                Surname= viewModel.Surname,
                Email= viewModel.Email,
                Subject= viewModel.Subject,
                CreationTime=DateTime.Now,
                RequestText=viewModel.Comment,
            };

            _context.ContactUsRequests.Add(request);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
        public IActionResult AboutUs()
        {
            AboutUsViewModel viewModel = new AboutUsViewModel();

            viewModel.TeamMembers = _context.TeamMembers.ToList();
            viewModel.Services=_context.Services.Where(x=>x.Type=="home").ToList();
            viewModel.AboutUsBanners = _context.Banners.Where(x => x.Type == "AboutUsBanner").ToList();
            viewModel.AboutUsSettings = _context.Settings.ToDictionary(x=>x.Key,x=>x.Value);

            return View(viewModel);
        }
        
    }
}