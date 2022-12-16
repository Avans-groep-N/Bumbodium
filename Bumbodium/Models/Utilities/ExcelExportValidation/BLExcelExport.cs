using Bumbodium.Data.DBModels;
using Bumbodium.Data.Repositories;
using System.Globalization;

namespace Bumbodium.WebApp.Models.Utilities.ExcelExportValidation
{
    public class BLExcelExport
    {
        private PresenceRepo _presenceRepo;

        public BLExcelExport(PresenceRepo presenceRepo)
        {
            _presenceRepo = presenceRepo;
        }

        public ExcelExportEmployeesHours GetEmployeesHours(int year, int weekNr)
        {
            var csv = new ExcelExportEmployeesHours() { Year = year, WeekNr = weekNr };
            FillEEEHours(csv);

            return csv;
        }

        private void FillEEEHours(ExcelExportEmployeesHours csv)
        {
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

            var firstdayOfWeek = FirstDateOfWeekISO8601(csv.Year, csv.WeekNr);
            var lastDayOfWeek = firstdayOfWeek.AddDays(7);
            var allWorkedHours = _presenceRepo.GetAllWorkedHoursInRange(firstdayOfWeek, lastDayOfWeek);

            /*foreach (var item in allWorkedHours)
            {
                if (item.AlteredClockInDateTime != null || item.AlteredClockOutDateTime != null)
                    FillAlteredHours(csv, item);
                else
                    FillHours(csv, item);
            }*/
        }

        private void FillAlteredHours(ExcelExportEmployeesHours csv, Presence item)
        {

        }

        private void FillHours(ExcelExportEmployeesHours csv, Presence item)
        {

        }







        private void Validation()
        {

        }

        private void AssignSurchargeRate()
        {

        }
        //TODO make this a separate class
        public DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            // Use first Thursday in January to get first week of the year as
            // it will never be in Week 52/53
            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            // As we're adding days to a date in Week 1,
            // we need to subtract 1 in order to get the right date for week #1
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }

            // Using the first Thursday as starting week ensures that we are starting in the right year
            // then we add number of weeks multiplied with days
            var result = firstThursday.AddDays(weekNum * 7);

            // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
            return result.AddDays(-3);
        }
    }
}
