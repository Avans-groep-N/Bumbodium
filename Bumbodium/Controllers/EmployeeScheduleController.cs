using Bumbodium.Data;
using Bumbodium.Data.DBModels;
using Bumbodium.WebApp.Models.Utilities.ClockingValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Bumbodium.WebApp.Models;
using Bumbodium.WebApp.Models.Utilities.ForecastValidation;
using System.Globalization;
using Bumbodium.WebApp.Models.Utilities.ExcelExportValidation;

namespace Bumbodium.WebApp.Controllers
{
    public class EmployeeScheduleController : Controller
    {

        private ShiftRepo _shiftRepo;
        private EmployeeRepo _employeeRepo;

        public EmployeeScheduleController(ShiftRepo shiftRepo, EmployeeRepo employeeRepo)
        {
            _shiftRepo = shiftRepo;
            _employeeRepo = employeeRepo;
        }

        public IActionResult Index()
        {
            DateTime weekStart = DateTime.Now;
            while (weekStart.DayOfWeek != DayOfWeek.Monday)
            {
                weekStart = weekStart.AddDays(-1);
            }
            DateTime weekEnd = weekStart.AddDays(7);
            List<Shift> shifts = _shiftRepo.GetShiftsInRange(weekStart, weekEnd, _employeeRepo.GetUserByName(User.Identity.Name).Id);
            WeekShiftsViewModel weekShifts = new WeekShiftsViewModel()
            {
                FirstDayOfWeek = weekStart
            };

            foreach (var s in shifts)
            {

                ShiftVM newShiftVM = new ShiftVM()
                {
                    StartTime = s.ShiftStartDateTime,
                    EndTime = s.ShiftEndDateTime,
                    EmployeeId = s.EmployeeId
                };

                weekShifts.AddShiftVM(newShiftVM);

            }
            return View(weekShifts);
        }

      /*  [HttpPost]
        public IActionResult SelectWeek(WeekShiftsViewModel model)
        {
            var shifts = _shiftRepo.GetShiftsInRange(model.FirstDayOfWeek, model.FirstDayOfWeek.AddDays(7), _employeeRepo.GetUserByName(User.Identity.Name).Id);

            foreach (var s in shifts)
            {

                ShiftVM newShiftVM = new ShiftVM()
                {
                    StartTime = s.ShiftStartDateTime,
                    EndTime = s.ShiftEndDateTime,
                    EmployeeId = s.EmployeeId
                };

                model.AddShiftVM(newShiftVM);

            }
            return View($"../{nameof(EmployeeScheduleController).Replace(nameof(Controller), "")}/{nameof(Index)}", model);
        }*/

    }
}
