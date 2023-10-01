using System.ComponentModel.DataAnnotations;

namespace Voxo.Models
{
    public class Size
    {
        public int Id { get; set; }
        [Required, MaxLength(10)]
        public string Name { get; set; }

        public List<ProductSize> ProductSizes { get; set; }
    }
}
