using Bumbodium.Data;
using Bumbodium.WebApp.Models.Utilities.ClockingValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data; 
using System.Globalization;
using Bumbodium.Data.DBModels;
using Microsoft.AspNetCore.Identity;

namespace Bumbodium.WebApp.Controllers
{
    [Authorize(Roles = "Employee")]
    public class ClockingEmployeeController : Controller
    {
        private BLClocking _blclocking;
        private EmployeeRepo _employeeRepo;

        public ClockingEmployeeController(BLClocking blclocking, EmployeeRepo employeeRepo)
        {
            _blclocking = blclocking;
            _employeeRepo = employeeRepo;
        }

        public IActionResult Index()
        {
            IdentityUser currentUser = _employeeRepo.GetUserByName(User.Identity.Name);
            var userId = currentUser.Id;
            var clockingViewModel = _blclocking.GetClockingViewModel(userId, ISOWeek.GetWeekOfYear(DateTime.Now), DateTime.Now.Year);
            return View(clockingViewModel);
        }

        [HttpPost]
        public IActionResult SelectWeek()
        {
            var id = Request.Form["Id"];

            if (id.Equals(""))
                return RedirectToAction(nameof(Index));

            var week = Request.Form["weeknumber"].First().Split("-W");
            int[] yearAndWeek = { Int32.Parse(week[0]), Int32.Parse(week[1]) };


            var clockingViewModel = _blclocking.GetClockingViewModel(id, yearAndWeek[1], yearAndWeek[0]);

            return View("../ClockingEmployee/Index", clockingViewModel);
        }
    }
}
