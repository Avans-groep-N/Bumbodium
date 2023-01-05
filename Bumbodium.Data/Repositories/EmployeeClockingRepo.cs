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
         
    }
}
