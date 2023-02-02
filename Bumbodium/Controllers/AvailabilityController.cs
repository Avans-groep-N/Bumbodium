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
    public class AvailabilityController : Controller
    {
        private readonly BumbodiumContext _ctx;
        //auto-suggested by Visual Studio. Make a class inhereting from IdentityUser.
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AvailabilityRepo _availabilityRepo;
        private readonly EmployeeRepo _employeeRepo;

        public AvailabilityController(BumbodiumContext ctx, UserManager<IdentityUser> userManager, AvailabilityRepo availabilityRepo)
        {
            _ctx = ctx;
            _userManager = userManager;
            _availabilityRepo = availabilityRepo;
            _employeeRepo = new EmployeeRepo(ctx);
        }
		
        [Authorize(Roles = "Employee")]
        public ActionResult Index()
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
		}

        [Authorize(Roles = "Manager")]
        public ActionResult Accodation()
        {
            var availabilities = _availabilityRepo.GetUnconfirmedAvailabilities();
            var model = new AvailabilityListViewModel()
            {
                Availabilities = new List<AvailabilityViewModel>()
            };

            //Converting DB models into view models
            foreach(var availability in availabilities)
            {
                var employee = _employeeRepo.GetEmployee(availability.EmployeeId);
                model.Availabilities.Add(new AvailabilityViewModel()
                {
                    Id = availability.AvailabilityId,
                    Employee = employee,
                    EmployeeId = employee.EmployeeID,
                    StartDateTime = availability.StartDateTime,
                    EndDateTime = availability.EndDateTime,
                    IsConfirmed = availability.IsConfirmed,
                    Type = availability.Type
                });
            }

            return View(model);
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

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public ActionResult Accodation(AvailabilityListViewModel model)
        {
            foreach(var availabilityVM in model.Availabilities)
            {
                //Converting VM to DBmodel
                var availability = new Availability()
                {
                    AvailabilityId = availabilityVM.Id,
                    EmployeeId = availabilityVM.EmployeeId,
                    StartDateTime = availabilityVM.StartDateTime,
                    EndDateTime = availabilityVM.EndDateTime,
                    Type = availabilityVM.Type,
                    IsConfirmed = availabilityVM.IsConfirmed
                };
                if (availability.IsConfirmed)
                {
                    _availabilityRepo.UpdateAvailability(availability);
                }
                else
                {
                    _availabilityRepo.DeleteAvailability(availability);
                }
            }
            return RedirectToAction("Accodation");
        }
    }
}