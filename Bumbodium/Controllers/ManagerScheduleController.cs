using Bumbodium.Data;
using Bumbodium.Data.DBModels;
using Bumbodium.Data.Repositories;
using Bumbodium.WebApp.Models.ManagerSchedule;
using Bumbodium.WebApp.Models.Utilities.ShiftValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Bumbodium.WebApp.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerScheduleController : Controller
    {
        private readonly EmployeeRepo _employeeRepo;
        private readonly ShiftRepo _shiftRepo;
        private readonly ForecastRepo _forecastRepo;

        public ManagerScheduleController(EmployeeRepo employeeRepo, ShiftRepo shiftRepo, ForecastRepo forecastRepo)
        {
            _employeeRepo = employeeRepo;
            _shiftRepo = shiftRepo;
            _forecastRepo = forecastRepo;
        }

        public IActionResult Index()
        {
            ManagerScheduleViewModel viewModel = new();
            DateTime t = DateTime.Today;
            viewModel.SelectedDate = t;
            viewModel.SelectedStartTime = new DateTime(t.Year, t.Month, t.Day, 8, 0, 0);
            viewModel.SelectedEndTime = new DateTime(t.Year, t.Month, t.Day, 22, 0, 0);
            viewModel = GetDataForViewModel(viewModel);
            return View("Index", viewModel);
        }

        [HttpPost]
        public IActionResult Index(ManagerScheduleViewModel viewModel)
        {
            DateTime d = viewModel.SelectedDate;
            DateTime st = viewModel.SelectedStartTime;
            DateTime et = viewModel.SelectedEndTime;
            // Swap StartTime and EndTime if StartTime is after EndTime
            if (st != et)
            {
                if (st < et)
                {
                    viewModel.SelectedStartTime = new(d.Year, d.Month, d.Day, st.Hour, st.Minute, 0);
                    viewModel.SelectedEndTime = new(d.Year, d.Month, d.Day, et.Hour, et.Minute, 0);
                }
                else
                {
                    viewModel.SelectedEndTime = new(d.Year, d.Month, d.Day, st.Hour, st.Minute, 0);
                    viewModel.SelectedStartTime = new(d.Year, d.Month, d.Day, et.Hour, et.Minute, 0);
                }
            }
            viewModel = GetDataForViewModel(viewModel);
            return View("Index", viewModel);
        }

        public IActionResult AddShift(ManagerScheduleViewModel viewModel)
        {
            DateTime d = viewModel.SelectedDate;
            DateTime st = viewModel.SelectedStartTime;
            DateTime et = viewModel.SelectedEndTime;
            viewModel.SelectedStartTime = new(d.Year, d.Month, d.Day, st.Hour, st.Minute, 0);
            viewModel.SelectedEndTime = new(d.Year, d.Month, d.Day, et.Hour, et.Minute, 0);

            if (viewModel.SelectedEmployeeId == null)
            {
                ModelState.AddModelError("NoEmployeeSelected", "Je moet een medewerker selecteren");
                viewModel = GetDataForViewModel(viewModel);
                return View("Index", viewModel);
            }
            Shift shift = new()
            {
                EmployeeId = viewModel.SelectedEmployeeId,
                ShiftStartDateTime = viewModel.SelectedStartTime,
                ShiftEndDateTime = viewModel.SelectedEndTime,
                DepartmentId = ((int)viewModel.SelectedDepartment + 1)
            };
            List<ValidationResult> validationResults = ShiftValidation.ValidateShift(_shiftRepo, shift).ToList();
            CaoInput cao = new(
                _employeeRepo.GetEmployee(viewModel.SelectedEmployeeId),
                _shiftRepo,
                shift);
            validationResults.AddRange(cao.ValidateRules());

            if (validationResults.Any())
            {
                foreach (ValidationResult result in validationResults)
                {
                    ModelState.AddModelError("ShiftValidationError", result.ErrorMessage);
                }
                viewModel = GetDataForViewModel(viewModel);
                return View("Index", viewModel);
            }

            _shiftRepo.InsertShift(shift);
            viewModel = GetDataForViewModel(viewModel);
            return View("Index", viewModel);
        }

        public IActionResult DeleteShift(ManagerScheduleViewModel viewModel, int ShiftId)
        {
            _shiftRepo.DeleteShift(ShiftId);
            viewModel = GetDataForViewModel(viewModel);
            return View("Index", viewModel);
        }

        // This needs to be called to fill the shifts and employees list with data
        private ManagerScheduleViewModel GetDataForViewModel(ManagerScheduleViewModel viewModel)
        {
            DateTime d = viewModel.SelectedDate;
            DateTime st = viewModel.SelectedStartTime;
            DateTime et = viewModel.SelectedEndTime;
            viewModel.SelectedStartTime = new(d.Year, d.Month, d.Day, st.Hour, st.Minute, 0);
            viewModel.SelectedEndTime = new(d.Year, d.Month, d.Day, et.Hour, et.Minute, 0);
            viewModel.AvailableEmployees = _employeeRepo.GetAvailableEmployees(((int)viewModel.SelectedDepartment + 1), viewModel.SelectedStartTime, viewModel.SelectedEndTime).ToList();
            viewModel.Shifts = _shiftRepo.GetShiftsInRange(viewModel.SelectedDate, viewModel.SelectedDate.AddDays(1), ((int)viewModel.SelectedDepartment + 1));
            viewModel.DepartmentViewModels = new();
            foreach (DepartmentType department in Enum.GetValues(typeof(DepartmentType)))
            {
                viewModel.DepartmentViewModels.Add(new()
                {
                    Type = department,
                    NeededHours = _forecastRepo.GetNeededHoursOfDepartmentOnDate(viewModel.SelectedDate, (int)department + 1),
                    PlannedHours = _shiftRepo.GetPlannedHoursOfDepartmentOnDate(viewModel.SelectedDate, (int)department + 1)
                });
            }

            return viewModel;
        }
    }
}
