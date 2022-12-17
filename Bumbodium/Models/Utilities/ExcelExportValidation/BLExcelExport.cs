using Bumbodium.Data;
using Bumbodium.Data.Repositories;
using Bumbodium.WebApp.Models.ExcelExport;
using System.Globalization;
using System.Xml.Linq;

namespace Bumbodium.WebApp.Models.Utilities.ExcelExportValidation
{
    public class BLExcelExport
    {
        private const int Addition0Percent = 0;
        private const int Addition33Percent = 33;
        private const int Addition50Percent = 50;
        private const double HourDif = 60.0;
        private const int Hour0Mark = 0;
        private const int Hour6Mark = 6;
        private const int Hour20Mark = 20;
        private const int Hour21Mark = 21;
        private const int Hour24Mark = 24;

        private PresenceRepo _presenceRepo;

        public BLExcelExport(PresenceRepo presenceRepo)
        {
            _presenceRepo = presenceRepo;
        }

        public ExcelExportEmployeesHours GetEmployeesHours(int year, int weekNr)
        {
            var employeeHoursDict = GetEEEHours(year, weekNr);

            var workedHoursList = FillEEEHours(employeeHoursDict);
            var employeesHours = new ExcelExportEmployeesHours() { Year = year, WeekNr = weekNr, EmployeeHours = workedHoursList };

            return employeesHours;
        }

        private Dictionary<string, EmployeeHours> GetEEEHours(int year, int weekNr)
        {
            var firstdayOfWeek = FirstDateOfWeekISO8601(year, weekNr);
            var lastDayOfWeek = firstdayOfWeek.AddDays(6);
            var allWorkedHours = _presenceRepo.GetAllWorkedHoursInRange(firstdayOfWeek, lastDayOfWeek);

            var employeesHoursSurchargeRate = new Dictionary<string, EmployeeHours>();

            foreach (var item in allWorkedHours)
            {
                var clockIn = item.ClockInDateTime;
                var clockOut = item.ClockOutDateTime;

                if (item.AlteredClockInDateTime != null)
                    clockIn = item.AlteredClockInDateTime.GetValueOrDefault();

                if (item.AlteredClockOutDateTime != null)
                    clockOut = item.AlteredClockOutDateTime.GetValueOrDefault();

                FillHours(employeesHoursSurchargeRate, item.EmployeeId, clockIn, clockOut);
            }
            return employeesHoursSurchargeRate;
        }

        public MemoryStream GetEmployeesHoursStream(ExcelExportEmployeesHours employeesHours)
        {
            var stream = new MemoryStream();
            using (var writeFile = new StreamWriter(stream, leaveOpen: true))
            {
                writeFile.WriteLine($"Yeer: {employeesHours.Year}; WeekNr: {employeesHours.WeekNr}");
                writeFile.WriteLine($"BID;Naam;Uren;Toeslag");
                foreach (var item in employeesHours.EmployeeHours)
                {
                    writeFile.WriteLine($"{item.EmployeeId};{item.EmployeeName};{item.WorkedHours};{item.SurchargeRate}%;");
                }
            }
            stream.Position = 0;

            return stream;
        }

        private List<CSVRowEmployee> FillEEEHours(Dictionary<string, EmployeeHours> hoursDict)
        {
            var workedHours = new List<CSVRowEmployee>();

            foreach (var employeeIDkey in hoursDict.Keys)
            {
                var key = hoursDict[employeeIDkey].EmployeeId;
                //TODO this has to by the EmployeeRepo
                string name = _presenceRepo.GetEmployeeName(key);

                AddToWorkedHours(workedHours, hoursDict, name, key + $":{Addition0Percent}");
                AddToWorkedHours(workedHours, hoursDict, name, key + $":{Addition33Percent}");
                AddToWorkedHours(workedHours, hoursDict, name, key + $":{Addition50Percent}");
            }
            return workedHours;
        }

        private static void AddToWorkedHours(List<CSVRowEmployee> workedHoursList, Dictionary<string, EmployeeHours> dict, string name, string key)
        {
            if (dict.ContainsKey(key))
            {
                workedHoursList.Add(new CSVRowEmployee()
                {
                    EmployeeId = dict[key].EmployeeId,
                    EmployeeName = name,
                    WorkedHours = dict[key].WorkedHours,
                    SurchargeRate = dict[key].SurchargeRate
                });
                dict.Remove(key);
            }
        }

        private void FillHours(Dictionary<string, EmployeeHours> workedHoursDict, string employeeId, DateTime clockIn, DateTime clockOut)
        {
            if (clockIn.Hour < Hour6Mark)
            {
                var morningShift = new DateTime(clockIn.Year, clockIn.Month, clockIn.Day, Hour6Mark, 0, 0).Subtract(clockIn);
                if (workedHoursDict.ContainsKey($"{employeeId}:{Addition50Percent}"))
                    workedHoursDict[$"{employeeId}:{Addition50Percent}"].WorkedHours += morningShift.Hours + (morningShift.Minutes / HourDif);
                else
                    workedHoursDict.Add($"{employeeId}:{Addition50Percent}", new EmployeeHours()
                    {
                        EmployeeId = employeeId,
                        WorkedHours = morningShift.Hours + (morningShift.Minutes / HourDif),
                        SurchargeRate = Addition50Percent
                    });
                clockIn = new DateTime(clockIn.Year, clockIn.Month, clockIn.Day, Hour6Mark, clockIn.Minute, clockIn.Second);
            }
            if (Hour6Mark <= clockIn.Hour && clockIn.Hour <= Hour20Mark)
            {
                var endTime = clockOut;
                if (Hour20Mark < clockOut.Hour)
                    endTime = new DateTime(clockOut.Year, clockOut.Month, clockOut.Day, Hour20Mark, 0, 0);

                var workedHours = endTime.Subtract(clockIn);
                if (workedHoursDict.ContainsKey($"{employeeId}:{Addition0Percent}"))
                    workedHoursDict[$"{employeeId}:{Addition0Percent}"].WorkedHours += workedHours.Hours + (workedHours.Minutes / HourDif);
                else
                    workedHoursDict.Add($"{employeeId}:{Addition0Percent}", new EmployeeHours()
                    {
                        EmployeeId = employeeId,
                        WorkedHours = workedHours.Hours + (workedHours.Minutes / HourDif),
                        SurchargeRate = Addition0Percent
                    });
            }
            if (clockIn.Hour < Hour20Mark && Hour20Mark <= clockOut.Hour)
            {
                var endTime = clockOut;
                if (Hour20Mark < clockOut.Hour)
                    endTime = new DateTime(clockOut.Year, clockOut.Month, clockOut.Day, Hour21Mark, 0, 0);

                var eveningShift = endTime.AddHours(-Hour20Mark);
                if (workedHoursDict.ContainsKey($"{employeeId}:{Addition33Percent}"))
                    workedHoursDict[$"{employeeId}:{Addition33Percent}"].WorkedHours += eveningShift.Hour + (eveningShift.Minute / HourDif);
                else
                    workedHoursDict.Add($"{employeeId}:{Addition33Percent}", new EmployeeHours()
                    {
                        EmployeeId = employeeId,
                        WorkedHours = eveningShift.Hour + (eveningShift.Minute / HourDif),
                        SurchargeRate = Addition33Percent
                    });
                clockIn = new DateTime(clockIn.Year, clockIn.Month, clockIn.Day, Hour21Mark, clockIn.Minute, clockIn.Second);
            }
            if (Hour21Mark < clockOut.Hour + (clockOut.Minute / HourDif))
            {
                var eveningShift = clockOut.AddHours(-Hour21Mark);
                if (workedHoursDict.ContainsKey($"{employeeId}:{Addition50Percent}"))
                    workedHoursDict[$"{employeeId}:{Addition50Percent}"].WorkedHours += eveningShift.Hour + (eveningShift.Minute / HourDif);
                else
                    workedHoursDict.Add($"{employeeId}:{Addition50Percent}", new EmployeeHours()
                    {
                        EmployeeId = employeeId,
                        WorkedHours = eveningShift.Hour + (eveningShift.Minute / HourDif),
                        SurchargeRate = Addition50Percent
                    });
            }
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
