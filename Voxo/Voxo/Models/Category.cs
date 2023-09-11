using System.ComponentModel.DataAnnotations;

namespace Voxo.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required,MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(20)]
        public string CategoryTag { get; set; }
        [MaxLength (100)]
        public string BackgroundImageName { get; set; }

        public List<Product> Products { get; set; }
    }
}
