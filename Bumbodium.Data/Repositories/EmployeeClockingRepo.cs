using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bumbodium.Data.DBModels;
using Bumbodium.Data.Interfaces;

namespace Bumbodium.Data.Repositories
{
    public class EmployeeClockingRepo
    {

        private BumbodiumContext _ctx;

        public EmployeeClockingRepo(BumbodiumContext ctx)
        {
            _ctx = ctx;
        }

        public List<Shift> GetShiftsFromWeek(string id, DateTime startTime, DateTime endTime)
        {
            return _ctx.Shift.Where(s => s.ShiftStartDateTime >= startTime  && s.ShiftEndDateTime <= endTime).ToList();
        }

        public List<Presence>GetPresenceFromWeek(string id, DateTime startTime, DateTime endTime)
        {
            return _ctx.Presence.Where(p =>
                   (p.ClockInDateTime >= startTime && p.AlteredClockInDateTime == null && p.ClockOutDateTime <= endTime && p.AlteredClockOutDateTime == null) ||

                   (p.ClockInDateTime >= startTime && p.AlteredClockInDateTime == null && p.AlteredClockOutDateTime <= endTime) ||

                   (p.AlteredClockInDateTime >= startTime && p.ClockOutDateTime <= endTime && p.AlteredClockOutDateTime == null) ||

                   (p.AlteredClockInDateTime >= startTime && p.AlteredClockOutDateTime <= endTime)
                   ).ToList();
        }
         
    }
}
