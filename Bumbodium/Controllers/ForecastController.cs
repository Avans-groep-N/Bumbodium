using Microsoft.AspNetCore.Mvc;
using Bumbodium.WebApp.Models;
using Bumbodium.WebApp.Models.Utilities.ForecastValidation;

namespace Bumbodium.WebApp.Controllers
{
    public class ForecastController : Controller
    {
        private BLForecast _blForecast;

        public ForecastController(BLForecast blforecast)
        {
            _blForecast = blforecast;
        }

        // GET: ForecastController
        public ActionResult Index()
        {
            var fw = new ForecastWeek() { WeekNr = 49 };
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

            /*_blForecast.GetForecastWeek(2022, 49);
*/
            return View(_blForecast.GetForecastWeek(2022, 51));
        }
    }
}
