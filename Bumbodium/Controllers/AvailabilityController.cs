using Bumbodium.Data.DBModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bumbodium.WebApp.Controllers
{
    [Authorize(Roles = "Employee")]
    public class AvailabilityController : Controller
    {
        // GET: AvailabilityController
        public ActionResult Index()
        {
            return View();
        }
    }
}
