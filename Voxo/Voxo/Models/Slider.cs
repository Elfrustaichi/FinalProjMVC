using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Voxo.Attributes.ValidationAttributes;

namespace Voxo.Models
{
    public class Slider
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public string SaleText { get; set; }
        [Required,MaxLength(20)]
        public string TitleText { get; set; }
        [MaxLength(20)]
        public string OfferText { get; set; }
        [MaxLength(100)]
        public string BackgroundImageName { get; set; }
        [MaxLength (100)]
        public string ButtonUrl { get; set; }
        [MaxLength(20)]
        public string ButtonHeaderText { get; set; }
        [MaxLength(20)]
        public string ButtonText { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        [NotMapped]
        [MaxFileSizeValidation(10485760)]
        [RequiredFileTypeValidation("image/png", "image/jpeg", "image/jpg")]
        public IFormFile SliderImage { get; set; }
    }
}
