using Bumbodium.Data.DBModels;
using Microsoft.EntityFrameworkCore;

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

        public List<Shift> GetShift(string id, DateTime dateTime)
        {
            return _ctx.Shift.Where(s => s.EmployeeId == id && s.ShiftStartDateTime.Day == dateTime.Day).ToList();
        }

        public Presence? GetStartToEndPresence(int Id)
        {
            return _ctx.Presence.FirstOrDefault(p => p.PresenceId == Id);
        }

        public List<Presence> GetWorkedHours(string id, DateTime dateTime)
        {
            return _ctx.Presence.Where(p => p.EmployeeId == id && p.ClockInDateTime.Date == dateTime.Date).ToList();
        }

        public void Save(Presence alterdPresence)
        {
            _ctx.Presence.Update(alterdPresence);
            _ctx.SaveChanges();
        }
    }
}
