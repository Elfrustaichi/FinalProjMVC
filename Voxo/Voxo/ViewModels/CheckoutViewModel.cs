using Voxo.Models;

namespace Voxo.ViewModels
{
    public class CheckoutViewModel
    {
        public Product Product { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
