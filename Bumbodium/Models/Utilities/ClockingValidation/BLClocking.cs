
using Bumbodium.Data;
using Bumbodium.Data.DBModels;
using Bumbodium.Data.Repositories;
using Bumbodium.WebApp.Models.ClockingView;
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

        public void Save(ClockingViewModel newClockTime)
        {
            
        }
        public ClockingViewModel GetClockingViewModel(string id, int weekNr, int year)
        {
            ClockingViewModel clockingVW = new ClockingViewModel() { YearNumber = year, WeekNumber = weekNr, EmployeeName = _employeeRepo.GetEmployee(id).FullName };

            clockingVW.ClockingDays = GetAllDaysOfWeek(id, weekNr, year);

            return clockingVW;
        }


        private List<ClockingDayViewModel> GetAllDaysOfWeek(string id, int weekNr, int year)
        {
            var clockday = new List<ClockingDayViewModel>();


            DateTime day = ISOWeek.ToDateTime(Convert.ToInt32(year), Convert.ToInt32(weekNr), DayOfWeek.Monday);

            for (int i = 0; i < DaysOftheWeek; i++)
            {
                var dayVW = new ClockingDayViewModel() { Day = day.AddDays(i) };
                dayVW.EmployeeClocking = GetAllShiftsOfDay(id, day.AddDays(i));
                if (dayVW.EmployeeClocking != null)
                    clockday.Add(dayVW);

            }

            return clockday;
        }

        private List<MedewerkerClockingItem>? GetAllShiftsOfDay(string id, DateTime dateTime)
        {
            List<Presence> listPresence = _presenceRepo.GetWorkedHours(id, dateTime);
            List<Shift> shifts = _presenceRepo.GetShift(id, dateTime);

            if (listPresence.Count == 0)
                return null;

            var dayItems = new List<MedewerkerClockingItem>();

            for (int i = 0; i < listPresence.Count; i++)
            {
                if (shifts.Count > i)
                    dayItems.Add(GetItem(listPresence[i], shifts[i]));
                else
                    dayItems.Add(GetItem(listPresence[i], null));
            }
            return dayItems;
        }

        private MedewerkerClockingItem GetItem(Presence presence, Shift? shift)
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
