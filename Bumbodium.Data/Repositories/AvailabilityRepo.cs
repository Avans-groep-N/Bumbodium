using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bumbodium.Data.DBModels;
using Bumbodium.Data.Interfaces;

namespace Bumbodium.Data
{
    public class AvailabilityRepo : IAvailabilityRepo
    {
        private BumbodiumContext _ctx;

        public AvailabilityRepo(BumbodiumContext ctx)
        {
            _ctx = ctx;
        }
        public List<Availability> GetAvailabilities()
        {
            return _ctx.Availability.ToList();
        }

        public List<Availability> GetAvailabilitiesInRange(DateTime start, DateTime end)
        {
            return _ctx.Availability.Where(a => a.StartDateTime > start && a.StartDateTime < end).ToList();
        }

        public List<Availability> GetUnconfirmedAvailabilities()
        {
            return _ctx.Availability.Where(a => a.IsConfirmed == false && a.Type != AvailabilityType.Schoolhours).ToList();
        }

        public void InsertAvailability(Availability availability)
        {
            _ctx.Availability.Add(availability);
            _ctx.SaveChanges();
        }

        public void DeleteAvailability(Availability availability)
        {
            _ctx.Availability.Remove(availability);
            _ctx.SaveChanges();
        }

        public void UpdateAvailability(Availability availability)
        {
            _ctx.Availability.Update(availability);
            _ctx.SaveChanges();
        }
    }
}
