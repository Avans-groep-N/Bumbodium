using Microsoft.AspNetCore.Mvc;
using Bumbodium.WebApp.Models;

namespace Bumbodium.WebApp.Controllers
{
    public class ForecastController : Controller
    {

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
    }
}
