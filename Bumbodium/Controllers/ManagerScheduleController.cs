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

namespace Bumbodium.WebApp.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerScheduleController : Controller
    {
        private readonly EmployeeRepo _employeeRepo;
        private readonly IAvailabilityRepo _availabilityRepo;
        private readonly IShiftRepo _shiftRepo;

        public ManagerScheduleController(EmployeeRepo employeeRepo, IAvailabilityRepo availabilityRepo, IShiftRepo shiftRepo)
        {
            _employeeRepo = employeeRepo;
            _availabilityRepo = availabilityRepo;
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
            viewModel.AvailableEmployees = _availabilityRepo.GetAvailableEmployees(1, viewModel.SelectedStartTime, viewModel.SelectedEndTime).ToList();
            viewModel.Shifts = _shiftRepo.GetShiftsInRange(t, t.AddDays(1), ((int)viewModel.SelectedDepartment + 1));
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
            viewModel.AvailableEmployees = _availabilityRepo.GetAvailableEmployees(((int)viewModel.SelectedDepartment + 1), viewModel.SelectedStartTime, viewModel.SelectedEndTime).ToList();
            viewModel.Shifts = _shiftRepo.GetShiftsInRange(viewModel.SelectedDate, viewModel.SelectedDate.AddDays(1), ((int)viewModel.SelectedDepartment + 1));
            return View("Index", viewModel);
        }

        public IActionResult AddShift(ManagerScheduleViewModel viewModel)
        {
            if(viewModel.SelectedEmployeeId == null)
            {
                ModelState.AddModelError("NoEmployeeSelected", "Je moet een medewerker selecteren");
                viewModel.AvailableEmployees = _availabilityRepo.GetAvailableEmployees(((int)viewModel.SelectedDepartment + 1), viewModel.SelectedStartTime, viewModel.SelectedEndTime).ToList();
                viewModel.Shifts = _shiftRepo.GetShiftsInRange(viewModel.SelectedDate, viewModel.SelectedDate.AddDays(1), ((int)viewModel.SelectedDepartment + 1));
                return View("Index", viewModel);
            }

            Shift shift = new()
            {
                EmployeeId = viewModel.SelectedEmployeeId,
                ShiftStartDateTime = viewModel.SelectedStartTime,
                ShiftEndDateTime = viewModel.SelectedEndTime,
                DepartmentId = ((int)viewModel.SelectedDepartment + 1)
            };
            // validate here
            _shiftRepo.InsertShift(shift);
            viewModel.AvailableEmployees = _availabilityRepo.GetAvailableEmployees(((int)viewModel.SelectedDepartment + 1), viewModel.SelectedStartTime, viewModel.SelectedEndTime).ToList();
            viewModel.Shifts = _shiftRepo.GetShiftsInRange(viewModel.SelectedDate, viewModel.SelectedDate.AddDays(1), ((int)viewModel.SelectedDepartment + 1));
            return View("Index", viewModel);
        }

        public IActionResult DeleteShift(ManagerScheduleViewModel viewModel, int ShiftId)
        {
            _shiftRepo.DeleteShift(ShiftId);
            viewModel.AvailableEmployees = _availabilityRepo.GetAvailableEmployees(((int)viewModel.SelectedDepartment + 1), viewModel.SelectedStartTime, viewModel.SelectedEndTime).ToList();
            viewModel.Shifts = _shiftRepo.GetShiftsInRange(viewModel.SelectedDate, viewModel.SelectedDate.AddDays(1), ((int)viewModel.SelectedDepartment + 1));
            return View("Index", viewModel);
        }
    }
}
