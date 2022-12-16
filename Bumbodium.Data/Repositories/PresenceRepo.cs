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
                        (p.ClockInDateTime >= startTime && 
                        p.ClockOutDateTime <= endTime && 
                        p.AlteredClockInDateTime == null && 
                        p.AlteredClockOutDateTime == null) ||
                        p.AlteredClockInDateTime >= startTime &&
                        p.AlteredClockOutDateTime <= endTime).ToList();
        }
    }
}
