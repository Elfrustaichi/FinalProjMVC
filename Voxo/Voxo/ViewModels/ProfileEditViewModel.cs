using System.ComponentModel.DataAnnotations;

namespace Voxo.ViewModels
{
    public class ProfileEditViewModel
    {
        [MaxLength(50)]
        public string Fullname { get; set; }
        [MaxLength(20)]
        public string Username { get; set; }
        [MaxLength (50)]
        public string Email { get; set; }
        [MaxLength (50)]
        public string Address { get; set; }

        public string PhoneNumber { get; set; }


    }
}
