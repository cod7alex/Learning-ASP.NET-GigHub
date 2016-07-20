namespace GigHub.Core.ViewModels
{
    public class GigDetailsViewModel
    {
        public string ArtistName { get; set; }

        public string ArtistId { get; set; }

        public string DateTimeString { get; set; }

        public string Venue { get; set; }

        public bool Going { get; set; }

        public bool Following { get; set; }

        public bool ShowActions { get; set; }
    }
}