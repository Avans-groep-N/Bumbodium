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
    public class ShiftRepo : IShiftRepo
    {
        private BumbodiumContext _ctx;

        public ShiftRepo(BumbodiumContext ctx)
        {
            _ctx = ctx;
        }

        public List<Shift> GetShiftsInRange(DateTime start, DateTime end)
        {
            return _ctx.Shift
                .Where(s => s.ShiftStartDateTime > start && s.ShiftStartDateTime < end)
                .Include(s => s.Employee)
                .ToList();
        }

        public void InsertShift(Shift shift)
        {
            _ctx.Shift.Add(shift);
            _ctx.SaveChanges();
        }

        public void DeleteShift(Shift shift)
        {
            _ctx.Shift.Remove(shift);
            _ctx.SaveChanges();
        }

        public void UpdateShift(Shift shift)
        {
            _ctx.Shift.Update(shift);
            _ctx.SaveChanges();
        }

        public List<Employee> GetEmployeesInRange(int departmentId, string? filter, int offset, int top)
        {
            departmentId++;
            return _ctx.Employee
                .Where(e => string.Concat(e.FirstName, e.MiddleName, e.LastName).Contains(filter))
                .Where(e => e.DateOutService == null)
                .Include(e => e.PartOFDepartment)
                .Where(e => e.PartOFDepartment.Any(pod => pod.DepartmentId == departmentId))
                .OrderBy(e => e.FirstName)
                .Skip(offset)
                .Take(top)
                .ToList();
        }

        public int GetEmployeeCount(int departmentId, string? filter)
        {
            departmentId++;
            return _ctx.Employee
                .Where(e => string.Concat(e.FirstName, e.MiddleName, e.LastName).Contains(filter))
                .Where(e => e.DateOutService == null)
                .Include(e => e.PartOFDepartment)
                .Where(e => e.PartOFDepartment.Any(pod => pod.DepartmentId == departmentId))
                .Count();
        }
    }
}
