using System.ComponentModel.DataAnnotations;

namespace Voxo.Models
{
    public class Brand
    {
        public int Id { get; set; }
        [Required,MaxLength(20)]
        public string Name { get; set; }

        public List<Product> Products { get; set; }
    }
}
