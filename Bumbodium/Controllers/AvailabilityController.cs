using Bumbodium.Data;
using Bumbodium.Data.DBModels;
using Bumbodium.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bumbodium.WebApp.Controllers
{
    public class AvailabilityController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AvailabilityRepo _availabilityRepo;
        private readonly EmployeeRepo _employeeRepo;

        public AvailabilityController(AvailabilityRepo availabilityRepo, EmployeeRepo employeeRepo, UserManager<IdentityUser> userManager)
        {
            _availabilityRepo = availabilityRepo;
            _employeeRepo = employeeRepo;
            _userManager = userManager;
        }

        [Authorize(Roles = "Employee")]
        public ActionResult Index()
        {
            string userId = _userManager.GetUserId(User);
            return View("Index", userId);
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
