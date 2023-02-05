using Bumbodium.Data;
using Bumbodium.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Radzen.Blazor.Rendering;
using System.Globalization;

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
            int weekNr = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            string selectedWeekString;
            if (weekNr < 10)
            {
                selectedWeekString = DateTime.Now.Year.ToString() + "-W0" + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            }
            else
            {
                selectedWeekString = DateTime.Now.Year.ToString() + "-W" + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            }

            WeekShiftsViewModel model = new WeekShiftsViewModel()
            {
                SelectedWeek = DateTime.Now,
                SelectedWeekString = selectedWeekString
            };
            model.Shifts = _shiftRepo.GetShiftsInRange(model.SelectedWeek.StartOfWeek(), model.SelectedWeek.EndOfWeek().AddHours(23).AddMinutes(59), _employeeRepo.GetUserByName(User.Identity.Name).Id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(WeekShiftsViewModel model)
        {
            model.SelectedWeek = CultureInfo.InvariantCulture.Calendar.AddWeeks(new DateTime(year: int.Parse(model.SelectedWeekString[..4]), 1, 1), int.Parse(model.SelectedWeekString.Substring(6, 2)));
            model.Shifts = _shiftRepo.GetShiftsInRange(model.SelectedWeek.StartOfWeek(), model.SelectedWeek.EndOfWeek().AddHours(23).AddMinutes(59), _employeeRepo.GetUser(User.Identity.Name).Id);
            return View($"../{nameof(EmployeeScheduleController).Replace(nameof(Controller), "")}/{nameof(Index)}", model);
        }

    }
}
