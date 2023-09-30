using Voxo.Models;

namespace Voxo.ViewModels
{
    public class ShopViewModel
    {
        public List<Category> Categories { get; set; } = new List<Category>();

        public List<Tag> Tags { get; set; }=new List<Tag>();

        public List<Brand> Brands { get; set; } = new List<Brand>();

        public PaginatedList<Product> Products { get; set;}

        public Banner ShopBanner { get; set; }

        public List<Product> PopularProducts { get; set;}
    }
}
