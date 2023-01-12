using Bumbodium.Data.DBModels;
using Bumbodium.Data.Repositories;
using Bumbodium.WebApp.Models.ClockingView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq.Dynamic.Core;

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
            ViewBag.Countrys = (Country[])Enum.GetValues(typeof(Country));
            
            return View(_standardsRepo.GetAll(country));
        }

        [HttpPost]
        public IActionResult SelectCountry()
        {
            var stringcountry = Request.Form["country"];
            var newCountry = Country.Netherlands;
            var countrys = Enum.GetValues(typeof(Country)).ToDynamicList();

            for (int i = 0; i < countrys.Count; i++)
            {
                if (countrys[i].ToString().Equals(stringcountry[0]))
                {
                    newCountry = countrys[i];
                    break;
                }
            }


            ViewBag.Countrys = countrys;
            return View("../Standard/Index", _standardsRepo.GetAll(newCountry));
        }
    }
}
