using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Runtime.InteropServices;

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
        [Required]
        public bool IsNewOffer { get; set; }
        [Required,MaxLength (20)]
        public string Type { get; set; }

        public DateTime OfferTime { get; set; }
        [Required]
        public bool IsLarge { get; set; }
        [MaxLength(100)]
        public string ButtonUrl { get; set; }
        [MaxLength(20)]
        public string ButtonText { get; set; }
        [Required,MaxLength(100)]
        public string BackgroundImageName { get; set; }
        [MaxLength(500)]
        public string Description { get; set;}

    }
}
