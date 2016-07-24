namespace GigHub.Core.Models
{
    public class Following
    {
        public ApplicationUser Artist { get; set; }

        public ApplicationUser Follower { get; set; }

        public string ArtistId { get; set; }

        public string FollowerId { get; set; }
    }
}