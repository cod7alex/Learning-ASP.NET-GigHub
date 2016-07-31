using GigHub.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GigHub.Core.Models
{
    public class Gig
    {
        public int Id { get; private set; }

        public bool IsCanceled { get; private set; }

        public string ArtistId { get; set; }

        public ApplicationUser Artist { get; set; }

        public DateTime DateTime { get; set; }

        public string Venue { get; set; }

        public byte GenreId { get; set; }

        public Genre Genre { get; set; }

        public ICollection<Attendance> Attendances { get; private set; }

        public Gig()
        {
            Attendances = new Collection<Attendance>();
        }

        public void Cancel()
        {
            IsCanceled = true;

            var notification = Notification.GigCancelled(this);

            foreach (var attendee in Attendances.Select(a => a.Attendee))
            {
                attendee.Notify(notification);
            }
        }

        public void Update(GigFormViewModel viewModel)
        {
            var notification = Notification.GigUpdated(this, DateTime, Venue);

            Venue = viewModel.Venue;
            DateTime = viewModel.GetDateTime();
            GenreId = viewModel.Genre;

            foreach (var attendee in Attendances.Select(a => a.Attendee))
            {
                attendee.Notify(notification);
            }
        }
    }
}