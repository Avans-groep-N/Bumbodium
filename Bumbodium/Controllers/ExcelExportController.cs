using Bumbodium.WebApp.Models.ExcelExport;
using Bumbodium.WebApp.Models.Utilities.ExcelExportValidation;
using Microsoft.AspNetCore.Mvc;
using Radzen.Blazor.Rendering;
using System.Globalization;

namespace Bumbodium.WebApp.Controllers
{
    public class ExcelExportController : Controller
    {
        private BLExcelExport _bLExcelExport;

        public ExcelExportController(BLExcelExport bLExcelExport)
        {
            _bLExcelExport = bLExcelExport;
        }

        public IActionResult Index()
        {
            DateTime date = DateTime.Now;
            var weekNr = ISOWeek.GetWeekOfYear(date);

            var workedHours = _bLExcelExport.GetEmployeesHours(date.Year, weekNr);
            return View(workedHours);
        }

        [HttpPost]
        public IActionResult SelectWeek()
        {
            var week = Request.Form["Week"].First().Split("-W");
            int[] yearAndWeek = { Int32.Parse(week[0]), Int32.Parse(week[1]) };

            var workedHours = _bLExcelExport.GetEmployeesHours(yearAndWeek[0], yearAndWeek[1]);
            //TODO look for another way to get to the view
            return View("../ExcelExport/Index", workedHours);
        }

        public ActionResult DownloadExcel(ExcelExportEmployeesHours employeesHours)
        {
            var employeesHoursPulsList = _bLExcelExport.GetEmployeesHours(employeesHours.Year, employeesHours.WeekNr);

            var stream = _bLExcelExport.GetEmployeesHoursStream(employeesHoursPulsList);
            return File(stream, "application/octet-stream", "Verloning.csv");
        }
    }
}
