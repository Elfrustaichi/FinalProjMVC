using Voxo.DAL;
using Voxo.Models;

namespace Voxo.Services
{
    public class LayoutService
    {
        private readonly VoxoContext _context;
        private readonly IHttpContextAccessor _contextAccessor;


        public LayoutService(VoxoContext context,IHttpContextAccessor httpContextAccessor) 
        { 
            _context = context;
            _contextAccessor = httpContextAccessor;
        }

        public Dictionary<string,string> GetSettings()
        {
            return _context.Settings.ToDictionary(x=>x.Key,x=>x.Value);
        }
        public List<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

    }
}
