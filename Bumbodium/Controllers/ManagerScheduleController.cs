using Bumbodium.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bumbodium.WebApp.Controllers
{
    public class ManagerScheduleController : Controller
    {
        public IActionResult Index()
        {
            return View(new DayShiftsViewModel());
        }
    }
}
