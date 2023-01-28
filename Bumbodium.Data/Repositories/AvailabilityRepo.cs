using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bumbodium.Data.DBModels;
using Bumbodium.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            return _ctx.Availability.Where(a => 
            (a.StartDateTime > start && a.StartDateTime < end) ||
            (a.EndDateTime > start && a.EndDateTime < end) ||
            (a.StartDateTime < start && a.EndDateTime > end)
            ).ToList();
        }
        public List<Availability> GetAvailabilitiesInRange(DateTime start, DateTime end, string userId)
        {
            return _ctx.Availability.Where(a => a.EmployeeId == userId)
                .Where(a =>
            (a.StartDateTime > start && a.StartDateTime < end) ||
            (a.EndDateTime > start && a.EndDateTime < end) ||
            (a.StartDateTime < start && a.EndDateTime > end)
            ).ToList();
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

        public IQueryable<Employee> GetAvailableEmployees(int departmentId, DateTime startTime, DateTime endTime)
        {
            IQueryable<Employee> employees = _ctx.Employee.AsQueryable();
            employees = employees.Where(e => e.PartOFDepartment.Any(pod => pod.DepartmentId == (int)(departmentId)));

            employees = employees.Include(e => e.Availability);
            employees = employees.Where(e => !e.Availability.Any(a =>
            (a.StartDateTime >= startTime && a.StartDateTime <= endTime) ||
            (a.EndDateTime >= startTime && a.EndDateTime <= endTime) ||
            (a.StartDateTime <= startTime && a.EndDateTime >= endTime)
            ));
            employees = employees.OrderBy(e => e.FirstName);
            return employees;
        }
    }
}
