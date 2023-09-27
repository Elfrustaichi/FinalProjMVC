using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Voxo.DAL;
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

            viewModel.Sliders=_context.Sliders.ToList();
            viewModel.NewArrivalBanners=_context.Banners.Where(x=>x.Type=="NewArrivalBanner").ToList();
            viewModel.NewArrivalProducts=_context.Products.Include(x=>x.ProductImages).Where(x=>x.IsNewArrival==true).ToList();
            viewModel.NewOfferBanners = _context.Banners.Where(x => x.Type == "NewOfferBanner").ToList();
            viewModel.LatestProducts=_context.Products.Include(x=>x.ProductImages).Where(x=>x.IsNewArrival==false).ToList();
            viewModel.InstagramBanners = _context.Banners.Where(x => x.Type == "InstagramBanner").ToList();
            viewModel.DealOfTheDayBanner = _context.Banners.FirstOrDefault(x=>x.Type=="DealOfTheDayBanner");
            viewModel.HomeServices = _context.Services.Where(x => x.Type == "Home").ToList();
            viewModel.TopCategories = _context.Categories.Where(x => x.CategoryTag == "New").ToList();

            return View(viewModel);
        }

        public IActionResult FAQ()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View("Error");
        }
    }
}