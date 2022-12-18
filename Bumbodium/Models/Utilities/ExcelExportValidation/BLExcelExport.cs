using Bumbodium.Data;
using Bumbodium.Data.Repositories;
using Bumbodium.WebApp.Models.ExcelExport;
using System;
using System.Globalization;
using System.Xml.Linq;

namespace Bumbodium.WebApp.Models.Utilities.ExcelExportValidation
{
    public class BLExcelExport
    {
        private const int Addition0Percent = 0;
        private const int Addition33Percent = 33;
        private const int Addition50Percent = 50;
        private const int Addition100Percent = 100;
        private const double HourDif = 60.0;
        private const int Hour0Mark = 0;
        private const int Hour6Mark = 6;
        private const int Hour18Mark = 18;
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
            var firstDayOfWeek = ISOWeek.ToDateTime(Convert.ToInt32(year), Convert.ToInt32(weekNr), DayOfWeek.Monday);

            var lastDayOfWeek = firstDayOfWeek.AddDays(7);
            var allWorkedHours = _presenceRepo.GetAllWorkedHoursInRange(firstDayOfWeek, lastDayOfWeek);

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
                writeFile.WriteLine($"Year: {employeesHours.Year}; WeekNr: {employeesHours.WeekNr}");
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
                AddToWorkedHours(workedHours, hoursDict, name, key + $":{Addition100Percent}");
            }
            return workedHours;
        }

        private void AddToWorkedHours(List<CSVRowEmployee> workedHoursList, Dictionary<string, EmployeeHours> dict, string name, string key)
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

        private void AddWorkedHoursToDict(Dictionary<string, EmployeeHours> workedHoursDict, TimeSpan workedHours, string employeeId, double additionPercent)
        {
            if (workedHoursDict.ContainsKey($"{employeeId}:{additionPercent}"))
                workedHoursDict[$"{employeeId}:{additionPercent}"].WorkedHours += workedHours.Hours + (workedHours.Minutes / HourDif);
            else
                workedHoursDict.Add($"{employeeId}:{additionPercent}", new EmployeeHours()
                {
                    EmployeeId = employeeId,
                    WorkedHours = workedHours.Hours + (workedHours.Minutes / HourDif),
                    SurchargeRate = additionPercent
                });
        }

        private void FillHours(Dictionary<string, EmployeeHours> workedHoursDict, string employeeId, DateTime clockIn, DateTime clockOut)
        {
            if (clockIn.DayOfWeek == DayOfWeek.Sunday)
            {
                var sundayShift = clockOut.Subtract(clockIn);
                AddWorkedHoursToDict(workedHoursDict,sundayShift,employeeId,Addition100Percent);
                return;
            }

            if (clockIn.Hour < Hour6Mark)
            {
                var morningShift = new DateTime(clockIn.Year, clockIn.Month, clockIn.Day, Hour6Mark, 0, 0).Subtract(clockIn);
                AddWorkedHoursToDict(workedHoursDict, morningShift, employeeId, Addition50Percent);
                clockIn = new DateTime(clockIn.Year, clockIn.Month, clockIn.Day, Hour6Mark, clockIn.Minute, clockIn.Second);
            }

            if (clockIn.DayOfWeek != DayOfWeek.Saturday)
            {
                if (Hour6Mark <= clockIn.Hour && clockIn.Hour <= Hour20Mark)
                {
                    var endTime = clockOut;
                    if (Hour20Mark < clockOut.Hour)
                        endTime = new DateTime(clockOut.Year, clockOut.Month, clockOut.Day, Hour20Mark, 0, 0);

                    var workedHours = endTime.Subtract(clockIn);
                    AddWorkedHoursToDict(workedHoursDict, workedHours, employeeId, Addition0Percent);
                }
                if (clockIn.Hour < Hour20Mark && Hour20Mark <= clockOut.Hour)
                {
                    var endTime = clockOut;
                    if (Hour20Mark < clockOut.Hour)
                        endTime = new DateTime(clockOut.Year, clockOut.Month, clockOut.Day, Hour21Mark, 0, 0);

                    var eveningShift = endTime.AddHours(-Hour20Mark);
                    var workedHours = endTime.Subtract(clockIn);
                    AddWorkedHoursToDict(workedHoursDict, workedHours, employeeId, Addition33Percent);

                    clockIn = new DateTime(clockIn.Year, clockIn.Month, clockIn.Day, Hour21Mark, clockIn.Minute, clockIn.Second);
                }
                if (Hour21Mark < clockOut.Hour + (clockOut.Minute / HourDif))
                {
                    var eveningShift = clockOut.Subtract(clockIn);
                    AddWorkedHoursToDict(workedHoursDict, eveningShift, employeeId, Addition50Percent);
                }
            }
            else
            {
                if (Hour6Mark <= clockIn.Hour && clockIn.Hour <= Hour18Mark)
                {
                    var endTime = clockOut;
                    if (Hour20Mark < clockOut.Hour)
                        endTime = new DateTime(clockOut.Year, clockOut.Month, clockOut.Day, Hour18Mark, 0, 0);

                    var workedHours = endTime.Subtract(clockIn);
                    AddWorkedHoursToDict(workedHoursDict, workedHours, employeeId, Addition0Percent);
                }

                if (Hour18Mark < clockOut.Hour + (clockOut.Minute / HourDif))
                {
                    var eveningShift = clockOut.AddHours(-Hour18Mark);
                    var workedHours = eveningShift.Subtract(clockIn);
                    AddWorkedHoursToDict(workedHoursDict, workedHours, employeeId, Addition50Percent);
                }
            }
        }
    }
}
