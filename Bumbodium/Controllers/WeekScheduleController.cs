using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bumbodium.Data.DBModels;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Bumbodium.WebApp.Controllers
{
    public class WeekScheduleController : Controller
    {
        // GET: WeekRoosterController
        [Authorize(Roles = "Manager")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
