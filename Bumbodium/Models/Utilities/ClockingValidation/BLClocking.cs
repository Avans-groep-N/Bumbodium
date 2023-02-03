using Bumbodium.Data;
using Bumbodium.Data.DBModels;
using Bumbodium.Data.Repositories;
using Bumbodium.WebApp.Models.ClockingView;

namespace Bumbodium.WebApp.Models.Utilities.ClockingValidation
{
    public class BLClocking
    {
        private PresenceRepo _presenceRepo;
        private EmployeeRepo _employeeRepo;

        public BLClocking(PresenceRepo presenceRepo, EmployeeRepo employeeRepo)
        {
            _presenceRepo = presenceRepo;
            _employeeRepo = employeeRepo;
        }

        #region EmployeeModel
        public List<EmployeeView> GetEmployees()
        {
            var employees = _employeeRepo.GetAllEmployees();

            var employeeList = new List<EmployeeView>();

            foreach (var employee in employees)
            {
                employeeList.Add(new EmployeeView() { Id = employee.EmployeeID, Name = employee.FullName, Email = employee.Email });
            }

            return employeeList;
        }

        public ClockingEmployeeViewModel GetEmployeeClockingViewModel(string? name, DateTime firstOfTheMonth)
        {
            firstOfTheMonth = firstOfTheMonth.AddDays(-firstOfTheMonth.Day + 1).Date;

            var clockingVW = new ClockingEmployeeViewModel() { FirstOfTheMonth = firstOfTheMonth };
            if (name == null)
                return clockingVW;

            string id = _employeeRepo.GetUserByName(name).Id;
            clockingVW.ClockingDays = GetDictOfEmployee(id, firstOfTheMonth);

            return clockingVW;
        }

        private Dictionary<DateTime, List<EmployeeClockingItem>> GetDictOfEmployee(string id, DateTime dateTime)
        {
            var clockdays = new Dictionary<DateTime, List<EmployeeClockingItem>>();

            List<Presence> listPresence = _presenceRepo.GetWorkedHours(id, dateTime);

            foreach (var presence in listPresence)
            {
                if (!clockdays.ContainsKey(presence.ClockInDateTime.Date))
                    clockdays[presence.ClockInDateTime.Date] = new List<EmployeeClockingItem>();
                clockdays[presence.ClockInDateTime.Date].Add(EmployeeGetItem(presence));
            }

            return clockdays;
        }

        private EmployeeClockingItem EmployeeGetItem(Presence presence)
        {
            var item = new EmployeeClockingItem();

            item.ClockStartTime = presence.AlteredClockInDateTime == null ? presence.ClockInDateTime : presence.AlteredClockInDateTime;
            item.ClockEndTime = presence.AlteredClockOutDateTime == null ? presence.ClockOutDateTime : presence.AlteredClockOutDateTime;

            item.IsChanged = presence.AlteredClockInDateTime != null || presence.AlteredClockOutDateTime != null;
            item.IsOnGoing = presence.ClockOutDateTime == null;

            return item;
        }
        #endregion


        #region ManagerModel
        public void Save(ClockingManagerViewModel clockingManagerVM)
        {
            var date = clockingManagerVM.ClockingDateTime;
            foreach (var clock in clockingManagerVM.ClockingDay)
            {
                foreach (var employeeClocking in clock.Value)
                {
                    var presenceDB = _presenceRepo.GetStartToEndPresence(employeeClocking.PresenceId);
                    employeeClocking.ClockStartTime = new DateTime(date.Year, date.Month, date.Day, employeeClocking.ClockStartTime.Hour, employeeClocking.ClockStartTime.Minute, 0);
                    if (employeeClocking.ClockEndTime.HasValue)
                        employeeClocking.ClockEndTime = new DateTime(date.Year, date.Month, date.Day, employeeClocking.ClockEndTime.Value.Hour, employeeClocking.ClockEndTime.Value.Minute, 0);
                    else
                        employeeClocking.ClockEndTime = new DateTime(date.Year, date.Month, date.Day, 23, 59, 0);
                    ChangePresenceDB(presenceDB, employeeClocking);
                }
            }
        }

        private void ChangePresenceDB(Presence? presenceDB, ManagerClockingItem employeeClocking)
        {
            if (presenceDB == null)
                return;
            if (presenceDB.ClockInDateTime != employeeClocking.ClockStartTime || presenceDB.ClockOutDateTime != employeeClocking.ClockEndTime || presenceDB.IsSick != employeeClocking.IsSick)
            {
                presenceDB.AlteredClockInDateTime = employeeClocking.ClockStartTime;
                presenceDB.AlteredClockOutDateTime = employeeClocking.ClockEndTime;
                presenceDB.IsSick = employeeClocking.IsSick;
                _presenceRepo.Save(presenceDB);
            }
        }

        public ClockingManagerViewModel GetManagerClockingViewModel(DateTime date)
        {
            var clock = new ClockingManagerViewModel() { ClockingDateTime = date};

            var allWorkedHoursPerDay = _presenceRepo.GetAllWorkedHoursInRange(date.Date, date.AddDays(1).Date);

            foreach (var presence in allWorkedHoursPerDay)
            {
                clock.AddToClockingDays(_employeeRepo.GetEmployee(presence.EmployeeId).FullName, new ManagerClockingItem()
                {
                    PresenceId = presence.PresenceId,
                    ClockStartTime = presence.AlteredClockInDateTime.HasValue ? presence.AlteredClockInDateTime.Value : presence.ClockInDateTime,
                    ClockEndTime = presence.AlteredClockOutDateTime.HasValue ? presence.AlteredClockOutDateTime.Value : presence.ClockOutDateTime,
                    IsSick = presence.IsSick,

                    //TODO koppel dit aan de shifts
                    ScheduleStartTime = DateTime.Now,
                    ScheduleEndTime = DateTime.Now
                }) ;
            }

            return clock;
        }

        internal void AddClocking(ManagerClockingItem model)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
