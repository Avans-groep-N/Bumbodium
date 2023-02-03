using Bumbodium.Data;
using Bumbodium.Data.DBModels;
using Bumbodium.WebApp.Models.Utilities.ClockingValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Bumbodium.WebApp.Models;

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
                weekStart.AddDays(-1);
            }
            DateTime weekEnd = weekStart.AddDays(7);
            List<Shift> shifts = _shiftRepo.GetShiftsInRange(weekStart, weekEnd, _employeeRepo.GetUserByName(User.Identity.Name).Id);
            WeekShiftsViewModel weekShifts = new WeekShiftsViewModel();
           
            foreach (var s in shifts)
            {
                ShiftVM tempShiftVM = new ShiftVM()
                {
                    StartTime = s.ShiftStartDateTime,
                    EndTime = s.ShiftEndDateTime,
                    EmployeeId = s.EmployeeId
                };

                weekShifts.AddShiftVM(tempShiftVM);

            }
            return View(weekShifts);
        }
    }
}
