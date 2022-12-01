using Bumbodium.Data.DBModels;
using Microsoft.AspNetCore.Mvc;

namespace Bumbodium.WebApp.Controllers
{
    public class StandardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
