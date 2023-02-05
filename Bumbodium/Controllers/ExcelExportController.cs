using Bumbodium.WebApp.Models.ExcelExport;
using Bumbodium.WebApp.Models.Utilities.ExcelExportValidation;
using Microsoft.AspNetCore.Mvc;

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
            return View(_bLExcelExport.GetEmployeesHours(DateTime.Now.AddMonths(-1)));
        }

        [HttpPost]
        public IActionResult Index(ExcelExportEmployeesHours workedHours)
        {
            return View(_bLExcelExport.GetEmployeesHours(workedHours.FirstDateOfMonth));
        }

        [HttpPost]
        public IActionResult SelectMonth(DateTime firstOfMonth)
        {
            var workedHours = _bLExcelExport.GetEmployeesHours(firstOfMonth);
            return RedirectToAction(nameof(Index), workedHours);
        }

        public ActionResult DownloadExcel(ExcelExportEmployeesHours employeesHours)
        {
            var employeesHoursPulsList = _bLExcelExport.GetEmployeesHours(employeesHours.FirstDateOfMonth);

            var stream = _bLExcelExport.GetEmployeesHoursStream(employeesHoursPulsList);

            string fileName = $"Verloning-{employeesHours.FirstDateOfMonth.Year}-{employeesHours.FirstDateOfMonth.Month}.csv";

            return File(stream, "application/octet-stream", fileName);
        }
    }
}
