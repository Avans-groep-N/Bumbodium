using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bumbodium.WebApp.Models;
using Bumbodium.Data.DBModels;
using Bumbodium.Data.Repositories;
using Bumbodium.Data;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Bumbodium.WebApp.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ForecastController : Controller
    {
        private readonly ForecastRepo _forecastRepo;

        public ForecastController(ForecastRepo forecastRepo)
        {
            _forecastRepo = forecastRepo;
        }

        // GET: ForecastController
        public ActionResult Index()
        {
            
            return View(_forecastRepo.GetAll());
        }

        // GET: ForecastController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ForecastController/Create
        public ActionResult Create()
        {
            return View(new ForecastWeekViewModel());
        }

        // POST: ForecastController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewForcast(ForecastWeekViewModel forecast)
        {
            try
            {
                _forecastRepo.CreateForecast(forecast.DaysOfTheWeek);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ForecastController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ForecastController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ForecastController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ForecastController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
