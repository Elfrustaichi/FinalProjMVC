using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Voxo.Attributes.ValidationAttributes;

namespace Voxo.Models
{
    public class Service
    {
        public int Id { get; set; }
        [Required,MaxLength(20)]
        public string Header { get; set; }
        [Required,MaxLength (100)]
        public string Text { get; set; }
        [MaxLength(100)]
        public string Icon { get; set; }
        [Required,MaxLength(25)]
        public string Type { get; set; }
        [NotMapped]
        [MaxFileSizeValidation(5242880)]
        [RequiredFileTypeValidation("image/jpeg","image/png","image/jpg")]
        public IFormFile IconImageFile { get; set; }
        




    }
}
