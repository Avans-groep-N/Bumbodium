using Bumbodium.Data;
using Bumbodium.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bumbodium.WebApp.Controllers
{
    public class EmployeeScheduleController : Controller
    {
        private readonly ShiftRepo _shiftRepo;

        public EmployeeScheduleController(ShiftRepo shiftRepo)
        {
            _shiftRepo = shiftRepo;
        }

        public IActionResult Index()
        {
            EmployeeScheduleViewModel viewModel = new();
            DateTime t = DateTime.Today;
            viewModel.SelectedDate = t;
            viewModel.Shifts = _shiftRepo.GetShiftsInRange(viewModel.SelectedDate, viewModel.SelectedDate.AddDays(7));
            return View("Index", viewModel);
        }
    }
}
