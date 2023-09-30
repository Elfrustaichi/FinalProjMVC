using System.ComponentModel.DataAnnotations;

namespace Voxo.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [MaxLength(20)]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [MaxLength(20)]
        [MinLength(8)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}
