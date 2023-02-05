namespace Bumbodium.WebApp.Models.ExcelExport
{
    public class ExcelExportEmployeesHours
    {
        public DateTime FirstDateOfMonth { get; set; }

        public List<CSVRowEmployee> EmployeeHours { get; set; }

        public ExcelExportEmployeesHours()
        {
            EmployeeHours = new List<CSVRowEmployee>();
        }
    }
}
