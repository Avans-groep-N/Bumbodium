using Bumbodium.Data;
using Bumbodium.Data.DBModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bumbodium.WebApp.Controllers
{
    [Authorize(Roles = "Employee")]
    public class AvailabilityController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AvailabilityController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }


        // GET: AvailabilityController
        public ActionResult Index()
        {
            string userId = _userManager.GetUserId(User);
            return View("Index",userId);
        }
    }
}
