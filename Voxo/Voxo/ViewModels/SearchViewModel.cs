using Voxo.Models;

namespace Voxo.ViewModels
{
    public class SearchViewModel
    {
        public string Search { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();

        public bool IsFound { get; set; }
    }
}
