using Bumbodium.Data.Repositories;
using Bumbodium.WebApp.Models;
using Bumbodium.WebApp.Models.Utilities.ForecastValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            return View(_blForecast.GetForecast(DateTime.Now));
        }

        public IActionResult ChangeInput(string id)
        {
            var datestring = id.Split("-W");
            var date = ISOWeek.ToDateTime(Convert.ToInt32(datestring[0]), Convert.ToInt32(datestring[1]), DayOfWeek.Monday);

            ForecastViewModel forecastVM = _blForecast.GetForecast(date);

            return View(forecastVM);
        }

        public IActionResult ChangeOutput(string id)
        {
            var datestring = id.Split("-W");
            var date = ISOWeek.ToDateTime(Convert.ToInt32(datestring[0]), Convert.ToInt32(datestring[1]), DayOfWeek.Monday);

            ForecastViewModel forecastVM = _blForecast.GetForecast(date);

            return View(forecastVM);
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
                _blForecast.ChangeOutputDB(viewModel);
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
                _blForecast.ChangeInputDB(viewModel);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public IActionResult SelectWeek()
        {
            var datestring = Request.Form["weeknumber"].First().Split("-W");
            var date = ISOWeek.ToDateTime(Convert.ToInt32(datestring[0]), Convert.ToInt32(datestring[1]), DayOfWeek.Monday);

            var fw = _blForecast.GetForecast(date);
            return View($"../{nameof(ForecastController).Replace(nameof(Controller), "")}/{nameof(Index)}", fw);
        }
    }
}
