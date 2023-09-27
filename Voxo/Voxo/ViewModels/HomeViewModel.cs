using Voxo.Models;

namespace Voxo.ViewModels
{
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set; }

        public List<Banner> NewArrivalBanners { get; set; }

        public List<Product> LatestProducts { get; set; }

        public List<Category> TopCategories { get; set; }

        public List<Banner> NewOfferBanners { get; set; }

        public List<Product> NewArrivalProducts { get; set;}

        public Banner DealOfTheDayBanner { get; set; }

        public List<Banner> InstagramBanners { get; set; }

        public List<Service> HomeServices { get; set; }


    }
}
