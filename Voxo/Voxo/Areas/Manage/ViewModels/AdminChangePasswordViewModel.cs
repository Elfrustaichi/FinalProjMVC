using System.ComponentModel.DataAnnotations;

namespace Voxo.Areas.Manage.ViewModels
{
    public class AdminChangePasswordViewModel
    {
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
