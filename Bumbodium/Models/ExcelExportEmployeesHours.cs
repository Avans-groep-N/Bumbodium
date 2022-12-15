namespace Bumbodium.WebApp.Models
{
    public class ExcelExportEmployeesHours
    {
        public int Year { get; set; }
        public int WeekNr { get; set; }

        public List<ExcelItemEmployeeHours> EmployeeHours { get; set; }

        public ExcelExportEmployeesHours()
        {
            EmployeeHours = new List<ExcelItemEmployeeHours>();
        }
    }
}
