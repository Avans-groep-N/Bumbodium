﻿using Bumbodium.Data.DBModels;
using Bumbodium.WebApp.Models;
using Bumbodium.WebApp.Models.Utilities.StandardsValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bumbodium.WebApp.Controllers
{
    [Authorize(Roles = "Manager")]
    public class StandardController : Controller
    {
        private const Country CountryOfFiliation = Country.Netherlands;
        private BLStandards _blStandards;

        public StandardController(BLStandards blStandards)
        {
            _blStandards = blStandards;
        }

        public IActionResult Index()
        {
            return View(_blStandards.GetStandardsViewModel(CountryOfFiliation));
        }

        [HttpPost]
        public IActionResult Index(StandardsViewModel standardsVM)
        {
            return View(_blStandards.GetStandardsViewModel(standardsVM.Country));
        }

        public IActionResult ChangeStandards(Country country)
        {
            return View(_blStandards.GetStandardsViewModel(country));
        }

        [HttpPost]
        public IActionResult ChangeStandards(StandardsViewModel standardsVM)
        {

            if (!ModelState.IsValid)
            {
                return View(_blStandards.GetStandardsViewModel(standardsVM.Country));
            }
            else
            {
                _blStandards.ChangeStandards(standardsVM);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
