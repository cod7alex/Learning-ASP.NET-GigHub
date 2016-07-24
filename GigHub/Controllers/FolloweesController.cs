using GigHub.Core;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers.Api
{
    public class FolloweesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FolloweesController(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            _unitOfWork = unitOfWork;
        }

        [Authorize]
        public ActionResult Following()
        {
            var artists = _unitOfWork.Followings.
                GetFollowedArtists(User.Identity.GetUserId())
                .ToList();

            return View(artists);
        }
    }
}