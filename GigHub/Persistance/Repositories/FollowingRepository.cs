using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System.Linq;

namespace GigHub.Persistance.Repositories
{
    public class FollowingRepository : IFollowingRepository
    {
        private ApplicationDbContext _context;

        public FollowingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Following GetFollowing(string userId, string artistId)
        {
            return _context.Followings
                .SingleOrDefault(f =>
                f.FollowerId == userId &&
                f.ArtistId == artistId);
        }
    }
}