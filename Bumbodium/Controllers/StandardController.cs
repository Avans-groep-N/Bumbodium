using Bumbodium.Data.DBModels;
using Bumbodium.Data.Repositories;
using Bumbodium.WebApp.Models;
using Bumbodium.WebApp.Models.Utilities.StandardsValidation;
using Microsoft.AspNetCore.Mvc;

namespace Bumbodium.WebApp.Controllers
{
    public class StandardController : Controller
    {
        private BLStandards _bLStandards;

        public StandardController(BLStandards bLStandards)
        {
            _bLStandards = bLStandards;
        }


        public IActionResult Index()
        {
            //TODO Make Reletive
            var country = Country.Netherlands;

            return View(_bLStandards.GetCountryStandards(country));
        }
    }
}
