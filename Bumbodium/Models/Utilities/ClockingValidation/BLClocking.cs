
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

        
        public ClockingViewModel GetClockingViewModel(string id, int weekNr, int year)
        {
            ClockingViewModel clockingVW = new ClockingViewModel() { YearNumber = year, WeekNumber = weekNr, EmployeeName = _employeeRepo.GetEmployee(id).FullName, EmployeeId = id};

            clockingVW.ClockingDays = GetAllDaysOfWeekManager(id, weekNr, year);

            return clockingVW;
        }

        public void Save(string employeeId, ManagerClockingItem mCItem)
        {
            var alterdPresence = _presenceRepo.GetStartToEndPresence(employeeId, mCItem.ClockStartTime, mCItem.ClockEndTime);

            alterdPresence.AlteredClockInDateTime = mCItem.AlterdClockStartTime;
            alterdPresence.AlteredClockOutDateTime = mCItem.AlterdClockEndTime;

            _presenceRepo.Save(alterdPresence);
        }

        private List<ClockingDayViewModel> GetAllDaysOfWeekManager(string id, int weekNr, int year)
        {
            var clockday = new List<ClockingDayViewModel>();


            DateTime day = ISOWeek.ToDateTime(Convert.ToInt32(year), Convert.ToInt32(weekNr), DayOfWeek.Monday);

            for (int i = 0; i < DaysOftheWeek; i++)
            {
                var dayVW = new ClockingDayViewModel() { Day = day.AddDays(i) };
                dayVW.ManagerClocking = GetAllShiftsOfDay(id, day.AddDays(i));
                if (dayVW.ManagerClocking != null)
                    clockday.Add(dayVW);

            }

            return clockday;
        }

        private List<ManagerClockingItem>? GetAllShiftsOfDay(string id, DateTime dateTime)
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

        private MedewerkerClockingItem MedewerkerGetItem(Presence presence, Shift? shift)
        {
            var item = new MedewerkerClockingItem();

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
    }
}
