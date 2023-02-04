using Bumbodium.Data;
using Bumbodium.WebApp.Models.ClockingView;
using Bumbodium.WebApp.Models.Utilities.ClockingValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bumbodium.WebApp.Controllers
{
    public class ClockingController : Controller
    {

        private BLClocking _blclocking;
        private EmployeeRepo _employeeRepo;

        public ClockingController(BLClocking blclocking, EmployeeRepo employeeRepo)
        {
            _blclocking = blclocking;
            _employeeRepo = employeeRepo;
        }

        #region Manager Clocking
        [Authorize(Roles = "Manager")]
        public IActionResult IndexManager()
        {
            var clockingViewModel = _blclocking.GetManagerClockingViewModel(DateTime.Now.AddDays(-1));
            return View(clockingViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public IActionResult IndexManager(ClockingManagerViewModel clockingManagerVM)
        {
            var clockingViewModel = _blclocking.GetManagerClockingViewModel(clockingManagerVM.ClockingDateTime);
            return View(clockingViewModel);
        }

        [Authorize(Roles = "Manager")]
        [Route("product/{date}/{presenceID}")]
        public IActionResult DeleteClocking(long date, int presenceId)
        {
            DateTime dateTime = new DateTime(date);
            _blclocking.DeleteClocking(dateTime, presenceId);
            var clockingViewModel = _blclocking.GetManagerClockingViewModel(dateTime.Date);

            return View($"../{nameof(ClockingController).Replace(nameof(Controller), "")}/{nameof(IndexManager)}", clockingViewModel);
        }

        [Authorize(Roles = "Manager")]
        public IActionResult AddClockingHour(DateTime date)
        {
            var options = new List<SelectListItem>();
            _employeeRepo.GetAllEmployees().ForEach(e => options.Add(new SelectListItem() { Value = e.FullName, Text = e.FullName }));

            ViewBag.Options = options;
            return View(new ManagerClockingItem() { Date = date, ClockStartTime = new DateTime(date.Year, date.Month, date.Day, 12, 0, 0), ClockEndTime = new DateTime(date.Year, date.Month, date.Day, 14, 0, 0) });
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public IActionResult AddClockingHour(ManagerClockingItem model)
        {
            if (!ModelState.IsValid)
            {
                var options = new List<SelectListItem>();
                _employeeRepo.GetAllEmployees().ForEach(e => options.Add(new SelectListItem() { Value = "1", Text = e.FullName }));

                ViewBag.Options = options;
                return View(model);
            }
            else
            {
                _blclocking.AddClocking(model);
                var clockingViewModel = _blclocking.GetManagerClockingViewModel(model.Date.Date);
                return View($"../{nameof(ClockingController).Replace(nameof(Controller), "")}/{nameof(IndexManager)}", clockingViewModel);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public IActionResult SaveNewTimes(ClockingManagerViewModel clockingManagerVM)
        {
            if (ModelState.IsValid)
            {
                _blclocking.Save(clockingManagerVM);
            }

            var clockingViewModel = _blclocking.GetManagerClockingViewModel(clockingManagerVM.ClockingDateTime.Date);

            return View($"../{nameof(ClockingController).Replace(nameof(Controller), "")}/{nameof(IndexManager)}", clockingViewModel);
        }
        #endregion


        #region Employee Clocking

        [Authorize(Roles = "Employee")]
        public IActionResult IndexEmployee()
        {
            var clockingEmployeeVM = _blclocking.GetEmployeeClockingViewModel(User.Identity?.Name, DateTime.Now);
            return View(clockingEmployeeVM);
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public IActionResult IndexEmployee(ClockingEmployeeViewModel clockingEmployeeVM)
        {
            return View(_blclocking.GetEmployeeClockingViewModel(User.Identity?.Name, clockingEmployeeVM.FirstOfTheMonth));
        }

        #endregion
    }
}
