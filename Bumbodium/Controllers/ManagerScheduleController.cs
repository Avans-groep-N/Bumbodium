using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bumbodium.Data.DBModels;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Bumbodium.Data;
using Bumbodium.Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using Bumbodium.WebApp.Models.ManagerSchedule;
using System.Security.Cryptography.Xml;
using Bumbodium.WebApp.Models.Utilities.ShiftValidation;

namespace Bumbodium.WebApp.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerScheduleController : Controller
    {
        private readonly EmployeeRepo _employeeRepo;
        private readonly IShiftRepo _shiftRepo;

        public ManagerScheduleController(EmployeeRepo employeeRepo, IShiftRepo shiftRepo)
        {
            _employeeRepo = employeeRepo;
            _shiftRepo = shiftRepo;
        }

        // GET: WeekRoosterController
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
            if(viewModel.SelectedEmployeeId == null)
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
            IEnumerable<ValidationResult> validationResults = ShiftValidation.ValidateShift(_shiftRepo, shift);
            if(validationResults.Any())
            {
                foreach (ValidationResult result in validationResults)
                {
                    ModelState.AddModelError("ShiftValidationError", result.ErrorMessage);
                }
                viewModel = GetDataForViewModel(viewModel);
                return View("Index", viewModel);
            }
            // TODO add CAO here
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
            viewModel.AvailableEmployees = _employeeRepo.GetAvailableEmployees(((int)viewModel.SelectedDepartment + 1), viewModel.SelectedStartTime, viewModel.SelectedEndTime).ToList();
            viewModel.Shifts = _shiftRepo.GetShiftsInRange(viewModel.SelectedDate, viewModel.SelectedDate.AddDays(1), ((int)viewModel.SelectedDepartment + 1));
            return viewModel;
        }
    }
}
