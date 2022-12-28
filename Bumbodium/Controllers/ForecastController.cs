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
        private readonly DepartmentRepo _departmentRepo;

        public ForecastController(ForecastRepo forecastRepo, DepartmentRepo departmentRepo)
        {
            _forecastRepo = forecastRepo;
            _departmentRepo = departmentRepo;
        }

        // GET: ForecastController
        public ActionResult Index()
        {
            var forecasts = _forecastRepo.GetAll();
            foreach(var forecast in forecasts)
            {
                forecast.Department = _departmentRepo.GetDepartmentById(forecast.DepartmentId);
            }
            return View(forecasts);
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
        public ActionResult CreateNewForcast(ForecastWeekViewModel forecastVM)
        {
            try
            {
                Forecast[] forecasts = new Forecast[7];

                //Transitioning VMs to dbmodels
                for (int index = 0; index < forecasts.Length; index++)
                {
                    var model = forecastVM.DaysOfTheWeek[index];
                    forecasts[index] = new Forecast()
                    {
                        Date = model.Date,
                        DepartmentId = forecastVM.DepartmentId,
                        AmountExpectedColis = model.AmountExpectedColis,
                        AmountExpectedCustomers = model.AmountExpectedCustomers,
                        AmountExpectedEmployees = model.AmountExpectedEmployees,
                        AmountExpectedHours = model.AmountExpectedHours
                    };
                }

                _forecastRepo.CreateForecast(forecasts);
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
