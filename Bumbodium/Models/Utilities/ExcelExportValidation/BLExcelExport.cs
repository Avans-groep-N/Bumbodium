using Bumbodium.Data;
using Bumbodium.Data.Repositories;
using Bumbodium.WebApp.Models.ExcelExport;
using System.Globalization;
using System.Xml.Linq;

namespace Bumbodium.WebApp.Models.Utilities.ExcelExportValidation
{
    public class BLExcelExport
    {
        private PresenceRepo _presenceRepo;

        public BLExcelExport(PresenceRepo presenceRepo)
        {
            _presenceRepo = presenceRepo;
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
        public ExcelExportEmployeesHours GetEmployeesHours(int year, int weekNr)
        {
            var hoursDict = GetEEEHours(year, weekNr);

            var workedHoursList = FillEEEHours(hoursDict);
            var emmployeesHours = new ExcelExportEmployeesHours() { Year = year, WeekNr = weekNr, EmployeeHours = workedHoursList };

            return emmployeesHours;
        }

        private List<CSVRowEmployee> FillEEEHours(Dictionary<string, EmployeeHours> hoursDict)
        {
            var workedHours = new List<CSVRowEmployee>();

            foreach (var employeeIDkey in hoursDict.Keys)
            {
                var key = hoursDict[employeeIDkey].EmployeeId;

                string name = _presenceRepo.GetEmployeeName(key);

                AddToWorkedHours(workedHours, hoursDict, name, key + $":{0}");
                AddToWorkedHours(workedHours, hoursDict, name, key + $":{33}");
                AddToWorkedHours(workedHours, hoursDict, name, key + $":{50}");
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

        private Dictionary<string, EmployeeHours> GetEEEHours(int year, int weekNr)
        {
            //var workedHoursList = new List<CSVRowEmployeeHours>();
            #region
            /*workedHoursList.Add(new ExcelItemEmployeeHours()
            {
                EmployeeId = "bd18f87f-7340-44a1-a739-97e67b8a4226",
                EmployeeName = "Maarten-Jan Kempernaar",
                WorkedHours = (30 + 30.0/ 60.0),
                SurchargeRate = 0
            });
            workedHoursList.Add(new ExcelItemEmployeeHours()
            {
                EmployeeId = "bd18f87f-7340-44a1-a739-97e67b8a4226",
                EmployeeName = "Maarten-Jan Kempernaar",
                WorkedHours = 4,
                SurchargeRate = 33
            });
            workedHoursList.Add(new ExcelItemEmployeeHours()
            {
                EmployeeId = "bd18f87f-7340-44a1-a739-97e67b8a4226",
                EmployeeName = "Maarten-Jan Kempernaar",
                WorkedHours = (2 + (30.0 / 60.0)),
                SurchargeRate = 50
            });
            workedHoursList.Add(new ExcelItemEmployeeHours()
            {
                EmployeeId = "4dc43714-5c7c-440a-9954-a696256013c2",
                EmployeeName = "Noa Veenboer",
                WorkedHours = 30,
                SurchargeRate = 0
            });
            workedHoursList.Add(new ExcelItemEmployeeHours()
            {
                EmployeeId = "4dc43714-5c7c-440a-9954-a696256013c2",
                EmployeeName = "Noa Veenboer",
                WorkedHours = 4 + 45.0 / 60.0,
                SurchargeRate = 33
            });
            workedHoursList.Add(new ExcelItemEmployeeHours()
            {
                EmployeeId = "4dc43714-5c7c-440a-9954-a696256013c2",
                EmployeeName = "Noa Veenboer",
                WorkedHours = 2,
                SurchargeRate = 50
            });
            workedHoursList.Add(new ExcelItemEmployeeHours()
            {
                EmployeeId = "db4fc362-868e-4bd0-9269-00cb17231a07",
                EmployeeName = "Gerry van Herk",
                WorkedHours = 30,
                SurchargeRate = 0
            });
            workedHoursList.Add(new ExcelItemEmployeeHours()
            {
                EmployeeId = "db4fc362-868e-4bd0-9269-00cb17231a07",
                EmployeeName = "Gerry van Herk",
                WorkedHours = 4,
                SurchargeRate = 33
            });
            workedHoursList.Add(new ExcelItemEmployeeHours()
            {
                EmployeeId = "db4fc362-868e-4bd0-9269-00cb17231a07",
                EmployeeName = "Gerry van Herk",
                WorkedHours = 2,
                SurchargeRate = 50
            });*/
            #endregion

            var firstdayOfWeek = FirstDateOfWeekISO8601(year, weekNr).AddDays(-3);
            var lastDayOfWeek = firstdayOfWeek.AddDays(7);
            var allWorkedHours = _presenceRepo.GetAllWorkedHoursInRange(firstdayOfWeek, lastDayOfWeek);

            var employeesHoursAndSurchargeRate = new Dictionary<string, EmployeeHours>();

            foreach (var item in allWorkedHours)
            {
                var clockIn = item.ClockInDateTime;
                var clockOut = item.ClockOutDateTime;

                if (item.AlteredClockInDateTime != null)
                    clockIn = item.AlteredClockInDateTime.GetValueOrDefault();

                if (item.AlteredClockOutDateTime != null)
                    clockOut = item.AlteredClockOutDateTime.GetValueOrDefault();

                FillHours(employeesHoursAndSurchargeRate, item.EmployeeId, clockIn, clockOut);
            }
            return employeesHoursAndSurchargeRate;
        }

        private void FillHours(Dictionary<string, EmployeeHours> workedHoursDict, string employeeId, DateTime clockIn, DateTime clockOut)
        {
            if (clockIn.Hour < 6)
            {
                var morningShift = new DateTime(clockIn.Year, clockIn.Month, clockIn.Day, 6, 0, 0).Subtract(clockIn);
                if (workedHoursDict.ContainsKey($"{employeeId}:{50}"))
                    workedHoursDict[$"{employeeId}:{50}"].WorkedHours += morningShift.Hours + (morningShift.Minutes / 60.0);
                else
                    workedHoursDict.Add($"{employeeId}:{50}", new EmployeeHours()
                    {
                        EmployeeId = employeeId,
                        WorkedHours = morningShift.Hours + (morningShift.Minutes / 60.0),
                        SurchargeRate = 50
                    });
                clockIn = new DateTime(clockIn.Year, clockIn.Month, clockIn.Day, 6, clockIn.Minute, clockIn.Second);
            }

            if (6 <= clockIn.Hour && clockIn.Hour <= 20)
            {
                var endTime = clockOut;
                if (20 < clockOut.Hour)
                    endTime = new DateTime(clockOut.Year, clockOut.Month, clockOut.Day, 20, 0, 0);

                var workedHours = endTime.Subtract(clockIn);
                if (workedHoursDict.ContainsKey($"{employeeId}:{0}"))
                    workedHoursDict[$"{employeeId}:{0}"].WorkedHours += workedHours.Hours + (workedHours.Minutes / 60.0);
                else
                    workedHoursDict.Add($"{employeeId}:{0}", new EmployeeHours()
                    {
                        EmployeeId = employeeId,
                        WorkedHours = workedHours.Hours + (workedHours.Minutes / 60.0),
                        SurchargeRate = 0
                    });
            }

            if (clockIn.Hour < 20 && 21 <= clockOut.Hour)
            {
                var eveningShift = clockOut.AddHours(-20);
                if (workedHoursDict.ContainsKey($"{employeeId}:{33}"))
                    workedHoursDict[$"{employeeId}:{33}"].WorkedHours += eveningShift.Hour + (eveningShift.Minute / 60.0);
                else
                    workedHoursDict.Add($"{employeeId}:{33}", new EmployeeHours()
                    {
                        EmployeeId = employeeId,
                        WorkedHours = eveningShift.Hour + (eveningShift.Minute / 60.0),
                        SurchargeRate = 33
                    });
                //clockIn = new DateTime(clockIn.Year, clockIn.Month, clockIn.Day, 21,0,0);
            }
            if (21 < clockOut.Hour)
            {
                var eveningShift = clockOut.AddHours(-21);
                if (workedHoursDict.ContainsKey($"{employeeId}:{50}"))
                    workedHoursDict[$"{employeeId}:{50}"].WorkedHours += eveningShift.Hour + (eveningShift.Minute / 60.0);
                else
                    workedHoursDict.Add($"{employeeId}:{50}", new EmployeeHours()
                    {
                        EmployeeId = employeeId,
                        WorkedHours = eveningShift.Hour + (eveningShift.Minute / 60.0),
                        SurchargeRate = 50
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
