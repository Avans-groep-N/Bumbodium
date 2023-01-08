
using Bumbodium.Data;
using Bumbodium.Data.DBModels;
using Bumbodium.Data.Repositories;
using Bumbodium.WebApp.Models.ClockingView;
using Microsoft.Extensions.Primitives;
using System.Globalization;

namespace Bumbodium.WebApp.Models.Utilities.ClockingValidation
{
    public class BLClocking
    {
        private const int DaysOftheWeek = 7;
        private PresenceRepo _presenceRepo;
        private EmployeeRepo _employeeRepo;

        public BLClocking(PresenceRepo presenceRepo,  EmployeeRepo employeeRepo)
        {
            _presenceRepo = presenceRepo;
            _employeeRepo = employeeRepo;
        }

        public List<EmployeeView> GetEmployees()
        {
            var employees = _employeeRepo.GetAllEmployees();

            var employeeList = new List<EmployeeView>();

            foreach (var employee in employees)
            {
                employeeList.Add(new EmployeeView() {Id = employee.EmployeeID, Name = employee.FullName, Email = employee.Email});
            }

            return employeeList;
        }

        public ClockingViewModel GetClockingViewModel(string? id, int weekNr, int year)
        {
            ClockingViewModel clockingVW = new ClockingViewModel() { YearNumber = year, WeekNumber = weekNr, EmployeeName = id, EmployeeId = id };
            if (id != "" && id != null)
                clockingVW = new ClockingViewModel() { YearNumber = year, WeekNumber = weekNr, EmployeeName = _employeeRepo.GetEmployee(id).FullName, EmployeeId = id};

            DateTime date = ISOWeek.ToDateTime(Convert.ToInt32(year), Convert.ToInt32(weekNr), DayOfWeek.Monday);

            clockingVW.ClockingDays = GetAllDaysOfWeek(id, date);

            return clockingVW;
        }

        public void Save(string employeeId, ManagerClockingItem mCItem)
        {
            var alterdPresence = _presenceRepo.GetStartToEndPresence(employeeId, mCItem.ClockStartTime, mCItem.ClockEndTime);
            if (alterdPresence == null)
                return;

            alterdPresence.AlteredClockInDateTime = mCItem.AlterdClockStartTime;
            alterdPresence.AlteredClockOutDateTime = mCItem.AlterdClockEndTime;

            _presenceRepo.Save(alterdPresence);
        }

        private List<ClockingDayViewModel> GetAllDaysOfWeek(string id, DateTime day)
        {
            var clockday = new List<ClockingDayViewModel>();

            for (int i = 0; i < DaysOftheWeek; i++)
            {
                var dayVW = new ClockingDayViewModel() { Day = day.AddDays(i) };
                dayVW.ManagerClocking = GetAllShiftsOfDayManager(id, day.AddDays(i));
                dayVW.EmployeeClocking = GetAllShiftsOfDayEmployee(id, day.AddDays(i));
                if (dayVW.ManagerClocking != null)
                    clockday.Add(dayVW);

            }

            return clockday;
        }

        private List<EmployeeClockingItem> GetAllShiftsOfDayEmployee(string id, DateTime dateTime)
        {
            List<Presence> listPresence = _presenceRepo.GetWorkedHours(id, dateTime);
            List<Shift> shifts = _presenceRepo.GetShift(id, dateTime);

            if (listPresence.Count == 0)
                return null;

            var dayItems = new List<EmployeeClockingItem>();

            for (int i = 0; i < listPresence.Count; i++)
            {
                if (shifts.Count > i)
                    dayItems.Add(EmployeeGetItem(listPresence[i], shifts[i]));
                else
                    dayItems.Add(EmployeeGetItem(listPresence[i], null));
            }
            return dayItems;
        }

        private List<ManagerClockingItem>? GetAllShiftsOfDayManager(string id, DateTime dateTime)
        {
            List<Presence> listPresence = _presenceRepo.GetWorkedHours(id, dateTime);
            List<Shift> shifts = _presenceRepo.GetShift(id, dateTime);

            if (listPresence.Count == 0)
                return null;

            var dayItems = new List<ManagerClockingItem>();

            for (int i = 0; i < listPresence.Count; i++)
            {
                if (shifts.Count > i)
                    dayItems.Add(ManagerGetItem(listPresence[i], shifts[i]));
                else
                    dayItems.Add(ManagerGetItem(listPresence[i], null));
            }
            return dayItems;
        }

        private EmployeeClockingItem EmployeeGetItem(Presence presence, Shift shift)
        {
            var item = new EmployeeClockingItem();

            item.ClockStartTime = presence.AlteredClockInDateTime == null ? presence.ClockInDateTime : presence.AlteredClockInDateTime;
            item.ClockEndTime = presence.AlteredClockOutDateTime == null ? presence.ClockOutDateTime : presence.AlteredClockOutDateTime;

            if (shift != null)
            {
                item.ScheduleStartTime = shift.ShiftStartDateTime;
                item.ScheduleEndTime = shift.ShiftEndDateTime;
            }
            item.IsChanged = presence.AlteredClockInDateTime != null || presence.AlteredClockOutDateTime != null;
            item.IsOnGoing = presence.ClockOutDateTime == null;

            return item;
        }

        private ManagerClockingItem ManagerGetItem(Presence presence, Shift? shift)
        {
            var item = new ManagerClockingItem();
            if (shift != null)
            {
                item.ScheduleStartTime = shift.ShiftStartDateTime;
                item.ScheduleEndTime = shift.ShiftEndDateTime;
            }

            item.ClockStartTime = presence.AlteredClockInDateTime == null ? presence.ClockInDateTime : presence.AlteredClockInDateTime;
            item.ClockEndTime = presence.AlteredClockOutDateTime == null ? presence.ClockOutDateTime : presence.AlteredClockOutDateTime;

            return item;
        }
    }
}
