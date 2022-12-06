using Bumbodium.Data.DBModels;
using Bumbodium.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bumbodium.WebApp.Controllers
{
    public class StandardController : Controller
    {
        private StandardsRepo _standardsRepo;
        public StandardController(StandardsRepo standardsRepo)
        {
            _standardsRepo = standardsRepo;
        }


        public IActionResult Index()
        {
            //TODO Make Reletive
            var country = Country.Netherlands;
            return View(_standardsRepo.GetAll(country));
        }
    }
}
