using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voxo.DAL;
using Voxo.Helpers;
using Voxo.Models;
using Voxo.ViewModels;

namespace Voxo.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "Admin")]
    public class TeamMemberController : Controller
    {
        private readonly VoxoContext _context;
        private readonly IWebHostEnvironment _env;

        public TeamMemberController(VoxoContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        //TeamMember index start
        public IActionResult Index(int page=1)
        {
            var query = _context.TeamMembers.AsQueryable();

            return View(PaginatedList<TeamMember>.Create(query,page,7));
        }
        //TeamMember index end

        //TeamMember create start
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(TeamMember member)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            if(_context.TeamMembers.Any(x=>x.Fullname==member.Fullname))
            {
                ModelState.AddModelError("Fullname","TeamMember already exists");
                return View();
            }
            if (member.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile","Team member must have photo");
            }
            if (member.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile","Image is required");
                return View();
            }
            member.ImageName = FileManager.Save(_env.WebRootPath, "uploads/TeamMemberImages", member.ImageFile);

            _context.TeamMembers.Add(member);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        //TeamMember create end

        //TeamMember delete start
        public IActionResult Delete(int id)
        {
            var existMember = _context.TeamMembers.Find(id);

            if (existMember == null)
            {
                return StatusCode(404);
            }
            string oldImage = existMember.ImageName;

            _context.TeamMembers.Remove(existMember);
            _context.SaveChanges();
            if (oldImage!=null)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/TeamMemberImages", oldImage);
            }
            
            return StatusCode(200);
        }
        //TeamMember delete end

        //TeamMember edit start
        public IActionResult Edit(int id)
        {
            var existMember=_context.TeamMembers.Find(id);
            if (existMember == null)
            {
                return StatusCode(404);
            }

            return View(existMember);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(TeamMember member)
        {
            var existMember = _context.TeamMembers.Find(member.Id);
            if(existMember == null)
            {
                return View("error");
            }
            if(!ModelState.IsValid)
            {
                return View(member);
            }
            if(existMember.Fullname!=member.Fullname&&_context.TeamMembers.Any(x=>x.Fullname==member.Fullname))
            {
                ModelState.AddModelError("Fullname", "Team member already exists");
            }

            existMember.Fullname = member.Fullname;
            existMember.Profession= member.Profession;
            existMember.Rate= member.Rate;
            existMember.Comment= member.Comment;
            existMember.OwnWebsite= member.OwnWebsite;

            string oldImage = null;
            if(member.ImageFile!=null)
            {
                oldImage = existMember.ImageName;
                existMember.ImageName = FileManager.Save(_env.WebRootPath,"uploads/TeamMemberImages",member.ImageFile);
            }

            _context.SaveChanges();

            if(oldImage!=null)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/TeamMemberImages", oldImage);
            }

            return RedirectToAction("index");
        }
        //TeamMember edit end

        public IActionResult Detail(int id)
        {
            var existMember = _context.TeamMembers.Find(id);

            if(existMember == null)
            {
                return StatusCode(404);
            }

            return View(existMember);
        }

    }
}
