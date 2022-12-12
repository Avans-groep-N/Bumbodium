using Microsoft.AspNetCore.Mvc;

namespace Bumbodium.WebApp.Controllers
{
    public class ClockingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
