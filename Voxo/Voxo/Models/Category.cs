using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Voxo.Attributes.ValidationAttributes;

namespace Voxo.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required, MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(20)]
        public string CategoryTag { get; set; }
        [MaxLength(100)]
        public string BackgroundImageName { get; set; }

        public List<Product> Products { get; set; }

        [NotMapped]
        [MaxFileSizeValidation(10485760)]
        [RequiredFileTypeValidation("image/png","image/jpeg","image/jpg")]
        public IFormFile CategoryImage { get; set; }
    }
}
