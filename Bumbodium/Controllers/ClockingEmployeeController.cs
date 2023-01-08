using Bumbodium.WebApp.Models.Utilities.ClockingValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace Bumbodium.WebApp.Controllers
{
    public class ClockingEmployeeController : Controller
    {
        private BLClocking _blclocking;

        public ClockingEmployeeController(BLClocking bLClocking)
        {
            _blclocking = bLClocking;
        }

        [Authorize(Roles = "Employee")]
        public IActionResult Index()
        {
            var clockingViewModel = _blclocking.GetClockingViewModel(GetEmployeeIdString(), ISOWeek.GetWeekOfYear(DateTime.Now), DateTime.Now.Year);
            return View(clockingViewModel);
        }

        [Authorize(Roles = "Employee")]
        [HttpPost]
        public IActionResult SelectWeek()
        {
            var week = Request.Form["weeknumber"].First().Split("-W");
            int[] yearAndWeek = { Int32.Parse(week[0]), Int32.Parse(week[1]) };


            var clockingViewModel = _blclocking.GetClockingViewModel(GetEmployeeIdString(), yearAndWeek[1], yearAndWeek[0]);

            return View("../ClockingEmployee/Index", clockingViewModel);
        }

        private string? GetEmployeeIdString()
        {
            var employeeList = _blclocking.GetEmployees();

            string? id = "";

            if (User.Identity != null)
                id = employeeList.FirstOrDefault(e => e.Email == User.Identity.Name)?.Id;
            return id;
        }
    }
}
