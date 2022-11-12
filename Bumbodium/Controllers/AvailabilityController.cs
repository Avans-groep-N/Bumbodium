using Bumbodium.Data;
using Microsoft.AspNetCore.Mvc;

namespace Bumbodium.WebApp.Controllers
{
    public class AvailabilityController : Controller
    {
        // GET: WeekRoosterController
        public ActionResult Index()
        {
            var Availability = new Availability[] { new Availability() {
                StartDateTime = DateTime.Now,
                EndDateTime = DateTime.Now.AddHours(1),
                Type = BeschikbaarheidType.Schoolhours} };
            return View(Availability);
        }
    }
}
