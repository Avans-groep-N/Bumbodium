using Bumbodium.Data;
using Bumbodium.Data.Repositories;
using Bumbodium.WebApp.Models.ExcelExport;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System.Globalization;

namespace Bumbodium.WebApp.Models.Utilities.ExcelExportValidation
{
    public class BLExcelExport
    {
        private const int Addition0Percent = 0;
        private const int Addition33Percent = 33;
        private const int Addition50Percent = 50;
        private const int Addition100Percent = 100;
        private const int AdditionSickPercent = 100;
        private const double HourDif = 60.0;

        private DateTime Hour6Mark = new DateTime(1, 1, 1, 5, 59, 0);
        private DateTime Hour18Mark = new DateTime(1, 1, 1, 17, 59, 0);
        private DateTime Hour20Mark = new DateTime(1, 1, 1, 19, 59, 0);
        private DateTime Hour21Mark = new DateTime(1, 1, 1, 20, 59, 0);
        private DateTime Hour24Mark = new DateTime(1, 1, 1, 23, 59, 0);

        private PresenceRepo _presenceRepo;
        private EmployeeRepo _employeeRepo;
        private List<DateTime> _holidays;

        public BLExcelExport(PresenceRepo presenceRepo, EmployeeRepo employeeRepo)
        {
            _presenceRepo = presenceRepo;
            _employeeRepo = employeeRepo;
            _holidays = new List<DateTime>();
        }

        public void AddToHolidays(DateTime holiday) => _holidays.Add(holiday);
        public void FillHolidays()
        {
            _holidays.Add(new DateTime(1, 1, 1));
            _holidays.Add(new DateTime(1, 12, 31));
            _holidays.Add(new DateTime(1, 4, 27));
            _holidays.Add(new DateTime(1, 4, 9));
            _holidays.Add(new DateTime(1, 12, 25));
        }

        public MemoryStream GetEmployeesHoursStream(ExcelExportEmployeesHours employeesHours)
        {
            var stream = new MemoryStream();
            using (var writeFile = new StreamWriter(stream, leaveOpen: true))
            {
                writeFile.WriteLine($"Jaar: {employeesHours.FirstDateOfMonth.Year}; Maand: {employeesHours.FirstDateOfMonth.Month}");
                writeFile.WriteLine($"BID;Naam;Uren;Toeslag");
                foreach (var item in employeesHours.EmployeeHours)
                {
                    writeFile.WriteLine($"{item.EmployeeId};{item.EmployeeName};{item.WorkedHours};{item.SurchargeRate}%;");
                }
            }
            stream.Position = 0;

            return stream;
        }

        public ExcelExportEmployeesHours GetEmployeesHours(DateTime firstOfMonth)
        {
            firstOfMonth = firstOfMonth.AddDays(-firstOfMonth.Day + 1).Date;

            var employeeHoursDict = GetEEEHours(firstOfMonth);

            var workedHoursList = FillEEEHours(employeeHoursDict);
            var employeesHours = new ExcelExportEmployeesHours() { FirstDateOfMonth = firstOfMonth, EmployeeHours = workedHoursList };

            return employeesHours;
        }

        private Dictionary<string, EmployeeHours> GetEEEHours(DateTime firstOfMonth)
        {
            var lastOfmonth = firstOfMonth.AddMonths(1);
            var allWorkedHoursDB = _presenceRepo.GetAllWorkedHoursInRange(firstOfMonth, lastOfmonth);

            var workHours = new Dictionary<string, EmployeeHours>();

            foreach (var item in allWorkedHoursDB)
            {
                var clockIn = item.ClockInDateTime;
                if (item.ClockOutDateTime == null)
                    continue;
                var clockOut = item.ClockOutDateTime.GetValueOrDefault();

                if (item.AlteredClockInDateTime != null)
                    clockIn = item.AlteredClockInDateTime.GetValueOrDefault();

                if (item.AlteredClockOutDateTime != null)
                    clockOut = item.AlteredClockOutDateTime.GetValueOrDefault();

                FillHours(workHours, item.EmployeeId, item.IsSick, clockIn, clockOut);
            }
            return workHours;
        }

        private List<CSVRowEmployee> FillEEEHours(Dictionary<string, EmployeeHours> hoursDict)
        {
            var workedHours = new List<CSVRowEmployee>();

            foreach (var employeeIDkey in hoursDict.Keys)
            {
                var key = hoursDict[employeeIDkey].EmployeeId;
                //TODO this has to by the EmployeeRepo
                string name = _employeeRepo.GetEmployee(key).FullName;

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
                    WorkedHours = Math.Round(dict[key].WorkedHours, 3),
                    SurchargeRate = dict[key].SurchargeRate
                });
                dict.Remove(key);
            }
        }

        private void FillHours(Dictionary<string, EmployeeHours> workedHoursDict, string employeeId, bool isSick, DateTime clockIn, DateTime clockOut)
        {
            if (isSick)
            {
                //these clock hours are the planned shift hours that have been put down in the present.
                var sickShift = clockOut.Subtract(clockIn);
                AddWorkedHoursToDict(workedHoursDict, sickShift, employeeId, AdditionSickPercent);
                return;
            }

            if (clockIn.DayOfWeek == DayOfWeek.Sunday || _holidays.Contains(clockIn.AddYears(clockIn.Year - 1).Date))
            {
                var sundayShift = clockOut.Subtract(clockIn);
                AddWorkedHoursToDict(workedHoursDict, sundayShift, employeeId, Addition100Percent);
                return;
            }

            var workedTimePerMark = IsInMarkedTime(Hour6Mark, clockIn, clockOut);
            if (workedTimePerMark.TotalMinutes > 0)
            {
                clockIn = clockIn.Add(workedTimePerMark);
                AddWorkedHoursToDict(workedHoursDict, workedTimePerMark, employeeId, Addition50Percent);
            }

            if (clockIn.DayOfWeek == DayOfWeek.Saturday)
            {
                workedTimePerMark = IsInMarkedTime(Hour18Mark, clockIn, clockOut);
                if (workedTimePerMark.TotalMinutes > 0)
                {
                    clockIn = clockIn.Add(workedTimePerMark);
                    AddWorkedHoursToDict(workedHoursDict, workedTimePerMark, employeeId, Addition0Percent);
                }
            }
            else
            {
                workedTimePerMark = IsInMarkedTime(Hour20Mark, clockIn, clockOut);
                if (workedTimePerMark.TotalMinutes > 0)
                {
                    clockIn = clockIn.Add(workedTimePerMark);
                    AddWorkedHoursToDict(workedHoursDict, workedTimePerMark, employeeId, Addition0Percent);
                }

                workedTimePerMark = IsInMarkedTime(Hour21Mark, clockIn, clockOut);
                if (workedTimePerMark.TotalMinutes > 0)
                {
                    clockIn = clockIn.Add(workedTimePerMark);
                    AddWorkedHoursToDict(workedHoursDict, workedTimePerMark, employeeId, Addition33Percent);
                }
            }

            workedTimePerMark = IsInMarkedTime(Hour24Mark, clockIn, clockOut);
            if (workedTimePerMark.TotalMinutes > 0)
            {
                clockIn = clockIn.Add(workedTimePerMark);
                AddWorkedHoursToDict(workedHoursDict, workedTimePerMark, employeeId, Addition50Percent);
            }
        }

        private TimeSpan IsInMarkedTime(DateTime hourMark, DateTime clockIn, DateTime clockOut)
        {
            var dateTimeMarkNow = new DateTime(clockIn.Year, clockIn.Month, clockIn.Day, hourMark.Hour, hourMark.Minute, hourMark.Second);
            if (clockIn > dateTimeMarkNow)
                return new TimeSpan(0);


            var endTimeOfmark = clockOut;
            if (clockOut > dateTimeMarkNow)
                endTimeOfmark = dateTimeMarkNow;

            var workedTime = endTimeOfmark.Subtract(clockIn);
            return workedTime;
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
    }
}
