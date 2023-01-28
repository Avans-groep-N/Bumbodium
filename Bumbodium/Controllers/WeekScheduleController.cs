using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bumbodium.Data.DBModels;
using Bumbodium.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Bumbodium.Data;
using System.Security.Cryptography.X509Certificates;
using Bumbodium.Data.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Bumbodium.WebApp.Controllers
{
    [Authorize(Roles = "Manager")]
    public class WeekScheduleController : Controller
    {
        private readonly EmployeeRepo _employeeRepo;
        private readonly IAvailabilityRepo _availabilityRepo;
        private readonly IShiftRepo _shiftRepo;

        public WeekScheduleController(EmployeeRepo employeeRepo, IAvailabilityRepo availabilityRepo, IShiftRepo shiftRepo)
        {
            _employeeRepo = employeeRepo;
            _availabilityRepo = availabilityRepo;
            _shiftRepo = shiftRepo;
        }

        // GET: WeekRoosterController
        public ActionResult Index()
        {
            ScheduleEmployeeListViewModel viewModel = new();
            DateTime t = DateTime.Today;
            viewModel.SelectedDate = t;
            viewModel.SelectedStartTime = new DateTime(t.Year, t.Month, t.Day, 8, 0, 0);
            viewModel.SelectedEndTime = new DateTime(t.Year, t.Month, t.Day, 22, 0, 0);
            viewModel.AvailableEmployees = _availabilityRepo.GetAvailableEmployees(1, viewModel.SelectedStartTime, viewModel.SelectedEndTime).ToList();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(ScheduleEmployeeListViewModel viewModel)
        {
            DateTime d = viewModel.SelectedDate;
            DateTime st = viewModel.SelectedStartTime;
            DateTime et = viewModel.SelectedEndTime;
            // Swap StartTime and EndTime if StartTime is after EndTime
            if(st != et)
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
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddShift(ScheduleEmployeeListViewModel viewModel)
        {

            return View(viewModel);
        }
    }
}
