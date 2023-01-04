using Bumbodium.Data.DBModels;
using Bumbodium.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Bumbodium.WebApp.Controllers
{
    [Authorize(Roles = "Manager")]
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
