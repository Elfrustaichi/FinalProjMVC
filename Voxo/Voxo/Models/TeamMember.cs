using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Voxo.Attributes.ValidationAttributes;

namespace Voxo.Models
{
    public class TeamMember
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Fullname { get; set; }
        [MaxLength(100)]
        public string ImageName { get; set; }
        [Required]
        [MaxLength(25)]
        public string Profession { get; set; }
        [Required]
        [Range(0, 5)]
        public int Rate { get; set; }
        [MaxLength(255)]
        public string Comment { get; set; }
        [MaxLength(20)]
        public string OwnWebsite { get; set; }
        [NotMapped]
        [MaxFileSizeValidation(31457280)]
        [RequiredFileTypeValidation("image/jpeg", "image/png", "image/jpg")]
        public IFormFile ImageFile { get; set; }
    }
}
