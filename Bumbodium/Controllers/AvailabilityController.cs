using Bumbodium.Data;
using Bumbodium.Data.DBModels;
using Bumbodium.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Bumbodium.WebApp.Controllers
{
    [Authorize(Roles = "Employee")]
    public class AvailabilityController : Controller
    {
        private readonly BumbodiumContext _ctx;
        //auto-suggested by Visual Studio. Make a class inhereting from IdentityUser.
        private readonly UserManager<IdentityUser> _userManager;

        public AvailabilityController(BumbodiumContext ctx, UserManager<IdentityUser> userManager)
        {
            _ctx = ctx;
            _userManager = userManager;
        }
        // GET: AvailabilityController
        public IActionResult Index()
        {
            return View(new AvailabilityViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(AvailabilityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(new AvailabilityViewModel());
            }
            IdentityUser user = _userManager.GetUserAsync(User).Result;

            Availability availability = new Availability
            {
                EmployeeId = user.Id,
                StartDateTime = model.Date.Add(model.StartTime),
                EndDateTime = model.Date.Add(model.EndTime)
            };

            _ctx.Availability.Add(availability);
            _ctx.SaveChanges();

            return View(model);
        }
    }
}