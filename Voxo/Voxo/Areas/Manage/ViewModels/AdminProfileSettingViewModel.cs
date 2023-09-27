using System.ComponentModel.DataAnnotations;

namespace Voxo.Areas.Manage.ViewModels
{
    public class AdminProfileSettingViewModel
    {
        [Required, MaxLength(20)]
        public string UserName { get; set; }
        [Required, MaxLength(50)]
        public string Fullname { get; set; }
        [Required, MaxLength(50)]
        public string Email { get; set; }
        [Required, DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
    }
}
