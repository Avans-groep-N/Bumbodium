using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bumbodium.Data.DBModels;
using Bumbodium.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Bumbodium.Data;

namespace Bumbodium.WebApp.Controllers
{
    [Authorize(Roles = "Manager")]
    public class WeekScheduleController : Controller
    {
        private readonly EmployeeRepo _employeeRepo;

        public WeekScheduleController(EmployeeRepo employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        // GET: WeekRoosterController
        public ActionResult Index()
        {
            ScheduleEmployeeListViewModel viewModel = new();
            DateTime t = DateTime.Today;
            viewModel.SelectedDate = t;
            viewModel.SelectedStartTime = new DateTime(t.Year, t.Month, t.Day, 8, 0, 0);
            viewModel.SelectedEndTime = new DateTime(t.Year, t.Month, t.Day, 22, 0, 0);
            // Do Employee get from post method
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(ScheduleEmployeeListViewModel viewModel)
        {
            DateTime d = viewModel.SelectedDate;
            DateTime st = viewModel.SelectedStartTime;
            DateTime et = viewModel.SelectedEndTime;
            // Swap StartTime and EndTime if StartTime is after EndTime
            if(st < et)
            {
                viewModel.SelectedStartTime = new(d.Year, d.Month, d.Day, st.Hour, st.Minute, 0);
                viewModel.SelectedEndTime = new(d.Year, d.Month, d.Day, et.Hour, et.Minute, 0);
            } else
            {
                viewModel.SelectedEndTime = new(d.Year, d.Month, d.Day, st.Hour, st.Minute, 0);
                viewModel.SelectedStartTime = new(d.Year, d.Month, d.Day, et.Hour, et.Minute, 0);
            }
            
            // Dirty db get
            IQueryable<Employee> employees = _employeeRepo.GetEmployees();
            // Department filter
            employees = employees.Where(e => e.PartOFDepartment.Any(pod => pod.DepartmentId == (int)(viewModel.SelectedDepartment + 1)));
            // Availability filter
            employees = employees.Include(e => e.Availability);
            employees = employees.Where(e => !e.Availability.Any(a =>
            (a.StartDateTime > viewModel.SelectedStartTime && a.StartDateTime < viewModel.SelectedEndTime) ||
            (a.EndDateTime > viewModel.SelectedStartTime && a.EndDateTime < viewModel.SelectedEndTime) ||
            (a.StartDateTime < viewModel.SelectedStartTime && a.EndDateTime > viewModel.SelectedEndTime)
            ));
            employees = employees.OrderBy(e => e.FirstName);
            viewModel.AvailableEmployees = employees.ToList();
            return View(viewModel);
        }
    }
}
