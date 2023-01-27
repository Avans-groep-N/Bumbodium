using Microsoft.AspNetCore.Mvc;
using Bumbodium.WebApp.Models;
using Bumbodium.Data.DBModels;
using Bumbodium.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Bumbodium.WebApp.Models.Utilities.ForecastValidation;
using System.Globalization;

namespace Bumbodium.WebApp.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ForecastController : Controller
    {
        private readonly ForecastRepo _forecastRepo;
        private readonly BLForecast _blForecast;
        private readonly DepartmentRepo _departmentRepo;

        public ForecastController(ForecastRepo forecastRepo, DepartmentRepo departmentRepo, BLForecast bLForecast)
        {
            _blForecast = bLForecast;
            _forecastRepo = forecastRepo;
            _departmentRepo = departmentRepo;
        }

        // GET: ForecastController
        public ActionResult Index()
        {
            return View(_blForecast.GetForecast(DateTime.Now));
        }

        [HttpPost]
        public IActionResult SelectWeek()
        {
            var datestring = Request.Form["weeknumber"].First().Split("-W");
            var date = ISOWeek.ToDateTime(Convert.ToInt32(datestring[0]), Convert.ToInt32(datestring[1]), DayOfWeek.Monday);
            
            var fw = _blForecast.GetForecast(date);
            return View($"../{nameof(ForecastController).Replace(nameof(Controller), "")}/{nameof(Index)}", fw);
        }


        public IActionResult ChangeInput(string id)
        {
            var datestring = id.Split("-W");
            var date = ISOWeek.ToDateTime(Convert.ToInt32(datestring[0]), Convert.ToInt32(datestring[1]), DayOfWeek.Monday);

            ForecastViewModel forecastVW = _blForecast.GetForecast(date);

            return View(forecastVW);
        }

        public IActionResult ChangeOutput(string id)
        {
            var datestring = id.Split("-W");
            var date = ISOWeek.ToDateTime(Convert.ToInt32(datestring[0]), Convert.ToInt32(datestring[1]), DayOfWeek.Monday);

            ForecastViewModel forecastVW = _blForecast.GetForecast(date);
            
            return View(forecastVW);
        }

        [HttpPost]
        public IActionResult ChangeOutput(ForecastViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            else
            {
                _blForecast.ChangeDB(viewModel);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public IActionResult ChangeInput(ForecastViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            else
            {
                //_blForecast.ChangeDB(viewModel);
                return RedirectToAction(nameof(Index));
            }
        }

        /*[HttpPost]
        public ActionResult SaveNewForecast()
        {
            var datestring = Request.Form["weeknumber"].First().Split("-W");
            var date = ISOWeek.ToDateTime(Convert.ToInt32(datestring[0]), Convert.ToInt32(datestring[1]), DayOfWeek.Monday);
            var amountExpectedEmployees = Request.Form["amountEmployees"];
            var amountExpectedHours = Request.Form["amountHours"];
            var amountExpectedCustomers = Request.Form["amountCustomers"];
            var amountExpectedColis = Request.Form["amountColis"];

            var forecastweek = new ForecastWeekViewModel() { WeekNr = ISOWeek.GetWeekOfYear(date), YearNr = date.Year };
            for (int i = 0; i < amountExpectedEmployees.Count; i++)
            {
                var fVW = new ForecastDayViewModel();
                fVW.Date = date.AddDays(i);
                fVW.AmountExpectedEmployees = Convert.ToInt32(amountExpectedEmployees[i]);
                fVW.AmountExpectedHours = Convert.ToInt32(amountExpectedHours[i]);
                fVW.AmountExpectedCustomers = Convert.ToInt32(amountExpectedCustomers[i]);
                fVW.AmountExpectedColis = Convert.ToInt32(amountExpectedColis[i]);
                forecastweek.DaysOfTheWeek[i] = fVW;
            }
            _blForecast.SaveForecast(forecastweek);


            var fw = GetForecastWeek();
            return View($"../{nameof(ForecastController).Replace(nameof(Controller), "")}/{nameof(Index)}", fw);
        }

        private ForecastWeekViewModel GetForecastWeek(DateTime? day = null)
        {
            string[] week = new string[2];

            if (day == null)
                week = Request.Form["weeknumber"].First().Split("-W");
            else
                week = new string[] { day.Value.Year.ToString(), ISOWeek.GetWeekOfYear(day.Value).ToString() };
            int[] yearAndWeek = { Int32.Parse(week[0]), Int32.Parse(week[1]) };
            var fw = _blForecast.GetForecastWeek(yearAndWeek[0], yearAndWeek[1]);
            return fw;
        }

        // POST: ForecastController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ForecastWeekViewModel forecastVM)
        {
            if (ModelState.IsValid)
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
                            AmountExpectedColis = model.AmountExpectedColis,
                            AmountExpectedCustomers = model.AmountExpectedCustomers
                        };
                    }

                    //_forecastRepo.CreateForecast(forecasts);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(new ForecastWeekViewModel());

        }*/
    }
}
