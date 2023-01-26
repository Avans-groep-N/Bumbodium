using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bumbodium.WebApp.Models;
using Bumbodium.Data.DBModels;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Bumbodium.WebApp.Models.ExcelExport;
using Bumbodium.Data;
using Microsoft.AspNetCore.Identity;

namespace Bumbodium.WebApp.Controllers
{
    [Authorize(Roles = "Manager")]
    public class WeekScheduleController : Controller
    {
        private readonly EmployeeRepo _employeeRepo;
        private readonly ShiftRepo _shiftRepo;

        public WeekScheduleController(EmployeeRepo employeeRepo, ShiftRepo shiftrepo)
        {
            _employeeRepo = employeeRepo;
            _shiftRepo = shiftrepo;
        }

        // GET: WeekRoosterController
        public ActionResult Index()
        {
            return View(new ScheduleEmployeeListViewModel());
        }

        [HttpPost]
        public ActionResult Index(ScheduleEmployeeListViewModel viewModel)
        {
            
            return View(viewModel);
        }
    }
}
