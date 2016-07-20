using GigHub.Core.Models;
using System.Collections.Generic;

namespace GigHub.Core.Repositories
{
    public interface IGigRepository
    {
        void AddGig(Gig gig);
        IEnumerable<Gig> GetArtistUpcomingGigs(string artistId);
        Gig GetGig(int id);
        IEnumerable<Gig> GetGigsUserIsAttending(string userId);
        Gig GetGigWithAttendees(int id);
    }
}