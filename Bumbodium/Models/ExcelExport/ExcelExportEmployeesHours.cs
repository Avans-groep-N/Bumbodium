namespace Bumbodium.WebApp.Models.ExcelExport
{
    public class ExcelExportEmployeesHours
    {
        public int Year { get; set; }
        public int WeekNr { get; set; }

        public List<CSVRowEmployee> EmployeeHours { get; set; }

        public ExcelExportEmployeesHours()
        {
            EmployeeHours = new List<CSVRowEmployee>();
        }
    }
}
