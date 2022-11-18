using Bumbodium.Data;
using Microsoft.AspNetCore.Mvc;

namespace Bumbodium.WebApp.Controllers
{
    public class AvailabilityController : Controller
    {
        // GET: AvailabilityController
        public ActionResult Index()
        {
            var Availability = new Availability[] { 
                new Availability() {
                StartDateTime = DateTime.Now,
                EndDateTime = DateTime.Now.AddHours(1),
                Type = AvailabilityType.Schoolhours} 
            };
            return View(Availability);
        }
    }
}
