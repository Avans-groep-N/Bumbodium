using Bumbodium.WebApp.Models;
using Bumbodium.WebApp.Models.Utilities.ExcelExportValidation;
using Microsoft.AspNetCore.Mvc;
using Radzen.Blazor.Rendering;
using System.Formats.Asn1;
using System.IO;

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
            return View(_bLExcelExport.GetEmployeesHours(2022,50));
        }

        public ActionResult DownloadExcel(ExcelExportEmployeesHours employeesHours)
        {
            var data = _bLExcelExport.GetEmployeesHours(employeesHours.Year, employeesHours.WeekNr);


            var stream = new MemoryStream();
            using (var writeFile = new StreamWriter(stream, leaveOpen: true))
            {
                writeFile.WriteLine($"Yeer: {data.Year}; WeekNr: {data.WeekNr}");
                writeFile.WriteLine($"BID;Naam;Uren;Toeslag");
                foreach (var item in data.EmployeeHours)
                {
                    writeFile.WriteLine($"{item.EmployeeId};{item.EmployeeName};{item.WorkedHours};{item.SurchargeRate}%;");
                }
            }
            stream.Position = 0;
            return File(stream, "application/octet-stream", "Verloning.csv"); ;
        }
    }
}
