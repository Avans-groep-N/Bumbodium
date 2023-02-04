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
                weekStart = weekStart.AddDays(-1);
            }
            DateTime weekEnd = weekStart.AddDays(7);
            List<Shift> shifts = _shiftRepo.GetShiftsInRange(weekStart, weekEnd, _employeeRepo.GetUserByName(User.Identity.Name).Id);
            WeekShiftsViewModel weekShifts = new WeekShiftsViewModel();
           
           /* foreach (var s in shifts)
            {*/

                Shift testShift = new Shift()
                {
                    EmployeeId = _employeeRepo.GetUserByName(User.Identity.Name).Id,
                    DepartmentId = 1,
                    ShiftStartDateTime = new DateTime(2023, 2, 4, 8, 0, 0),
                    ShiftEndDateTime = new DateTime(2023, 2, 4, 12, 0, 0)
                };

                ShiftVM newShiftVM = new ShiftVM()
                {
                    StartTime = testShift.ShiftStartDateTime,
                    EndTime = testShift.ShiftEndDateTime,
                    EmployeeId = testShift.EmployeeId
                };

                weekShifts.AddShiftVM(newShiftVM);

          /*  }*/
            return View(weekShifts);
        }
    }
}
