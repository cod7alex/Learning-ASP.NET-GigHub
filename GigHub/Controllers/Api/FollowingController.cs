using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Persistance;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers
{
    [Authorize]
    public class FollowingController : ApiController
    {
        public ApplicationDbContext _context;

        public FollowingController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Follow(FollowingDto dto)
        {
            var userId = User.Identity.GetUserId();

            if (_context.Followings.Any(f => f.ArtistId == dto.ArtistId && f.FollowerId == userId))
                return BadRequest("The following already exists");

            var following = new Following
            {
                ArtistId = dto.ArtistId,
                FollowerId = userId
            };

            _context.Followings.Add(following);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Unfollow(string id)
        {
            var userId = User.Identity.GetUserId();

            var following = _context.Followings
                .SingleOrDefault(f => f.ArtistId == id && f.FollowerId == userId);

            if (following == null)
                return NotFound();

            _context.Followings.Remove(following);
            _context.SaveChanges();

            return Ok(id);
        }
    }
}
