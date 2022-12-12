using Microsoft.AspNetCore.Mvc;
using Bumbodium.WebApp.Models;

namespace Bumbodium.WebApp.Controllers
{
    public class ForecastController : Controller
    {
        /*private readonly ForecastRepo _forecastRepo;

        public ForecastController(ForecastRepo forecastRepo)
        {
            _forecastRepo = forecastRepo;
        }*/


        // GET: ForecastController
        public ActionResult Index()
        {
            var fw = new ForecastWeek() { WeekNr = 34 };
            fw.Days.Add(new ForecastItems()
            {
                Date = new DateTime(2020, 8, 23),
                AmountEmployees = 8,
                AmountColis = 8,
                AmountCustommers = 8,
                AmountHours = 8
            });
            fw.Days.Add(new ForecastItems()
            {
                Date = new DateTime(2020, 8, 23),
                AmountEmployees = 8,
                AmountColis = 8,
                AmountCustommers = 8,
                AmountHours = 8
            });
            fw.Days.Add(new ForecastItems()
            {
                Date = new DateTime(2020, 8, 23),
                AmountEmployees = 8,
                AmountColis = 8,
                AmountCustommers = 8,
                AmountHours = 8
            });
            fw.Days.Add(new ForecastItems()
            {
                Date = new DateTime(2020, 8, 23),
                AmountEmployees = 8,
                AmountColis = 8,
                AmountCustommers = 8,
                AmountHours = 8
            });
            fw.Days.Add(new ForecastItems()
            {
                Date = new DateTime(2020, 8, 23),
                AmountEmployees = 8,
                AmountColis = 8,
                AmountCustommers = 8,
                AmountHours = 8
            });
            fw.Days.Add(new ForecastItems()
            {
                Date = new DateTime(2020, 8, 23),
                AmountEmployees = 8,
                AmountColis = 8,
                AmountCustommers = 8,
                AmountHours = 8
            });
            fw.Days.Add(new ForecastItems()
            {
                Date = new DateTime(2020, 8, 23),
                AmountEmployees = 8,
                AmountColis = 8,
                AmountCustommers = 8,
                AmountHours = 8
            });

            return View(fw);
        }

        // GET: ForecastController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ForecastController/Create
        public ActionResult Create()
        {
            return View(new ForecastWeek());
        }

        // POST: ForecastController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewForcast(ForecastWeek forecast)
        {
            try
            {
                //_forecastRepo.CreateForecast(forecast.DaysOfTheWeek);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
