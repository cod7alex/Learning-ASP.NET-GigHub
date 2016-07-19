using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers.Api
{
    public class FolloweesController : Controller
    {
        private ApplicationDbContext _context;

        public FolloweesController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize]
        public ActionResult Following()
        {
            var userId = User.Identity.GetUserId();

            var artists = _context.Followings
                .Where(f => f.FollowerId == userId)
                .Select(f => f.Artist)
                .ToList();

            return View(artists);
        }
    }
}