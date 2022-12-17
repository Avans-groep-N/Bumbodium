using Bumbodium.WebApp.Models.ExcelExport;
using Bumbodium.WebApp.Models.Utilities.ExcelExportValidation;
using Microsoft.AspNetCore.Mvc;
using Radzen.Blazor.Rendering;

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
            var workedHours = _bLExcelExport.GetEmployeesHours(2021, 1);
            return View(workedHours);
        }

        public ActionResult DownloadExcel(ExcelExportEmployeesHours employeesHours)
        {
            var employeesHoursPulsList = _bLExcelExport.GetEmployeesHours(employeesHours.Year, employeesHours.WeekNr);

            var stream = _bLExcelExport.GetEmployeesHoursStream(employeesHoursPulsList);
            return File(stream, "application/octet-stream", "Verloning.csv");
        }

        
    }
}
