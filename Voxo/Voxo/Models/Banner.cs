using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Runtime.InteropServices;
using Voxo.Attributes.ValidationAttributes;

namespace Voxo.Models
{
    public class Banner
    {
        public int Id { get; set; }
        [Required,MaxLength(20)]
        public string Header { get; set; }
        [MaxLength(20)]
        public string UltraBannerSecondHeader { get; set; }
        [MaxLength (20)]
        public string Offer { get; set; }
        [MaxLength(20)]
        public string DiscountText { get; set; }
        [Column(TypeName = "money")]
        public decimal Price { get; set; }
        [Required,MaxLength (20)]
        public string Type { get; set; }

        public DateTime OfferTime { get; set; }
        public string Size { get; set; }
        [MaxLength(100)]
        public string ButtonUrl { get; set; }
        [MaxLength(20)]
        public string ButtonText { get; set; }
        [MaxLength(100)]
        public string BackgroundImageName { get; set; }
        [MaxLength(500)]
        public string Description { get; set;}

        [NotMapped]
        [MaxFileSizeValidation(31457280)]
        [RequiredFileTypeValidation("image/jpeg","image/png","image/jpg")]
        public IFormFile BackgroundImage { get; set; }

    }
}
