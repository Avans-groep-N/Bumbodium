using Bumbodium.Data;
using Bumbodium.Data.DBModels;
using Bumbodium.Data.Repositories;
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
        private readonly AvailabilityRepo _availabilityRepo;

        public AvailabilityController(BumbodiumContext ctx, UserManager<IdentityUser> userManager, AvailabilityRepo availabilityRepo)
        {
            _ctx = ctx;
            _userManager = userManager;
            _availabilityRepo = availabilityRepo;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAvailability(AvailabilityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(new AvailabilityViewModel());
            }
            IdentityUser user = _userManager.GetUserAsync(User).Result;

            Availability availability = new Availability
            {
                EmployeeId = user.Id,
                StartDateTime = model.Date.ToDateTime(model.StartTime),
                EndDateTime = model.Date.ToDateTime(model.EndTime),
                Type = model.AvailabilityType
            };

            _ctx.Availability.Add(availability);
            _ctx.SaveChanges();

            return View(model);
            try
            {
                _availabilityRepo.InsertAvailability(availability);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Delete(int id)
        {
            Availability availability = _ctx.Availability.Find(id);
            if (availability == null)
            {
                //returns a 404 error
                return NotFound();
            }
            IdentityUser user = _userManager.GetUserAsync(User).Result;

            if (availability.EmployeeId != user.Id)
            {
                //returns a 401 error
                return Unauthorized();
            }
            //TODO: add a pop-up to ask if they want to confirm the deletion
            _ctx.Availability.Remove(availability);
            _ctx.SaveChanges();

            //TODO: return a message saying what was removed
            return RedirectToAction("Index");
        }

        public IActionResult Update(int id, Availability model)
        {
            IdentityUser user = _userManager.GetUserAsync(User).Result;

            model.EmployeeId = user.Id;

            if (model.EmployeeId != user.Id)
            {
                //returns a 401 error
                return Unauthorized();
            }

            _ctx.Availability.Update(model);
            _ctx.SaveChanges();

            //TODO: return a message saying what was updated
            return RedirectToAction("Index");
        }
    }
}