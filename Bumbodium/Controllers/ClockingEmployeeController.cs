using Bumbodium.Data;
using Bumbodium.WebApp.Models.Utilities.ClockingValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Globalization;
using Bumbodium.Data.DBModels;
using Microsoft.AspNetCore.Identity;
using Bumbodium.WebApp.Models.ClockingView;

namespace Bumbodium.WebApp.Controllers
{
    [Authorize(Roles = "Employee")]
    public class ClockingEmployeeController : Controller
    {
        private BLClocking _blclocking;

        public ClockingEmployeeController(BLClocking blclocking)
        {
            _blclocking = blclocking;
        }

        public IActionResult Index()
        {
            var clockingEmployeeVM = _blclocking.GetEmployeeClockingViewModel(User.Identity?.Name, DateTime.Now);
            return View(clockingEmployeeVM);
        }

        [HttpPost]
        public IActionResult Index(ClockingEmployeeViewModel clockingEmployeeVM)
        {
            return View(_blclocking.GetEmployeeClockingViewModel(User.Identity?.Name, clockingEmployeeVM.FirstOfTheMonth));
        }
    }
}