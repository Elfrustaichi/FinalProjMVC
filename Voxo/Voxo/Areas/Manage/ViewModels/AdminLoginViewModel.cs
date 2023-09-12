using System.ComponentModel.DataAnnotations;

namespace Voxo.Areas.Manage.ViewModels
{
    public class AdminLoginViewModel
    {
        [Required]
        [MaxLength(20)]
        public string Username { get; set; }
        [Required]
        [MaxLength (20)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
