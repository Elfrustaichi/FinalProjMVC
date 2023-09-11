using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace Voxo.Models
{
    public class TeamMember
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Fullname { get; set; }
        [Required]
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
    }
}
