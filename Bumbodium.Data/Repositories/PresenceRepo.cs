using Bumbodium.Data.DBModels;

namespace Bumbodium.Data.Repositories
{
    public class PresenceRepo
    {
        private BumbodiumContext _ctx;

        public PresenceRepo(BumbodiumContext ctx)
        {
            _ctx = ctx;
        }

        public List<Presence> GetAllWorkedHoursInRange(DateTime startTime, DateTime endTime)
        {
            return _ctx.Presence.Where(p =>
                    (p.ClockInDateTime >= startTime && p.AlteredClockInDateTime == null && p.ClockOutDateTime <= endTime && p.AlteredClockOutDateTime == null) ||

                    (p.ClockInDateTime >= startTime && p.AlteredClockInDateTime == null && p.AlteredClockOutDateTime <= endTime) ||

                    (p.AlteredClockInDateTime >= startTime && p.ClockOutDateTime <= endTime && p.AlteredClockOutDateTime == null) ||

                    (p.AlteredClockInDateTime >= startTime && p.AlteredClockOutDateTime <= endTime)
                    ).ToList();
        }

        //TODO refactor to EmployeeRepo when that is fixed
        public string GetEmployeeName(string id)
        {
            return _ctx.Employee.First(e => e.EmployeeID.Equals(id)).FullName;
        }
    }
}
