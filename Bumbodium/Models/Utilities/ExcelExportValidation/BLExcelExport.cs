namespace Bumbodium.WebApp.Models.Utilities.ExcelExportValidation
{
    public class BLExcelExport
    {
        public BLExcelExport()
        {

        }

        public ExcelExportEmployeesHours GetEmployeesHours(int year, int weekNr)
        {
            var csv = new ExcelExportEmployeesHours() { Year = year, WeekNr = weekNr };
            csv.EmployeeHours.Add(new ExcelItemEmployeeHours()
            {
                EmployeeId = "bd18f87f-7340-44a1-a739-97e67b8a4226",
                EmployeeName = "Maarten-Jan Kempernaar",
                WorkedHours = 30,
                SurchargeRate = 0
            });
            csv.EmployeeHours.Add(new ExcelItemEmployeeHours()
            {
                EmployeeId = "bd18f87f-7340-44a1-a739-97e67b8a4226",
                EmployeeName = "Maarten-Jan Kempernaar",
                WorkedHours = 4,
                SurchargeRate = 33
            });
            csv.EmployeeHours.Add(new ExcelItemEmployeeHours()
            {
                EmployeeId = "bd18f87f-7340-44a1-a739-97e67b8a4226",
                EmployeeName = "Maarten-Jan Kempernaar",
                WorkedHours = 2,
                SurchargeRate = 50
            });
            csv.EmployeeHours.Add(new ExcelItemEmployeeHours()
            {
                EmployeeId = "4dc43714-5c7c-440a-9954-a696256013c2",
                EmployeeName = "Noa Veenboer",
                WorkedHours = 30,
                SurchargeRate = 0
            });
            csv.EmployeeHours.Add(new ExcelItemEmployeeHours()
            {
                EmployeeId = "4dc43714-5c7c-440a-9954-a696256013c2",
                EmployeeName = "Noa Veenboer",
                WorkedHours = 4,
                SurchargeRate = 33
            });
            csv.EmployeeHours.Add(new ExcelItemEmployeeHours()
            {
                EmployeeId = "4dc43714-5c7c-440a-9954-a696256013c2",
                EmployeeName = "Noa Veenboer",
                WorkedHours = 2,
                SurchargeRate = 50
            });
            csv.EmployeeHours.Add(new ExcelItemEmployeeHours()
            {
                EmployeeId = "db4fc362-868e-4bd0-9269-00cb17231a07",
                EmployeeName = "Gerry van Herk",
                WorkedHours = 30,
                SurchargeRate = 0
            });
            csv.EmployeeHours.Add(new ExcelItemEmployeeHours()
            {
                EmployeeId = "db4fc362-868e-4bd0-9269-00cb17231a07",
                EmployeeName = "Gerry van Herk",
                WorkedHours = 4,
                SurchargeRate = 33
            });
            csv.EmployeeHours.Add(new ExcelItemEmployeeHours()
            {
                EmployeeId = "db4fc362-868e-4bd0-9269-00cb17231a07",
                EmployeeName = "Gerry van Herk",
                WorkedHours = 2,
                SurchargeRate = 50
            });

            return csv;
        }
    }
}
