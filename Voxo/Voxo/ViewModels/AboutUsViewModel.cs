using Voxo.Models;

namespace Voxo.ViewModels
{
    public class AboutUsViewModel
    {
        public List<Banner> AboutUsBanners { get; set; }

        public List<Service> Services { get; set;}

        public List<TeamMember> TeamMembers { get; set; }

        public Dictionary<string,string> AboutUsSettings { get; set; }
    }
}
