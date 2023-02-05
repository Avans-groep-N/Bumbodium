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
        public List<Shift> GetShiftsInRange(DateTime start, DateTime end, int departmentId)
        {
            return _ctx.Shift
                .Where(s => s.DepartmentId == departmentId)
                .Where(s => s.ShiftStartDateTime > start && s.ShiftStartDateTime < end)
                .Include(s => s.Employee)
                .ToList();
        }
        public List<Shift> GetShiftsInRange(DateTime start, DateTime end, string employeeId)
        {
            return _ctx.Shift
                .Where(s => s.EmployeeId == employeeId)
                .Where(s => s.ShiftStartDateTime > start && s.ShiftStartDateTime < end)
                .Include(s => s.Employee)
                .OrderBy(s => s.ShiftStartDateTime)
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
        
        public void DeleteShift(int shiftId)
        {
            Shift shift = _ctx.Shift.Where(e => e.ShiftId == shiftId).FirstOrDefault();
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

        public bool ShiftExistsInTime(DateTime start, DateTime end, string employeeId)
        {
            return _ctx.Shift.Any(a =>
            (a.EmployeeId == employeeId) &&
            ((a.ShiftStartDateTime > start && a.ShiftStartDateTime < end) ||
            (a.ShiftEndDateTime > start && a.ShiftEndDateTime < end) ||
            (a.ShiftStartDateTime < start && a.ShiftEndDateTime > end))
            ); 
        }

        public double GetPlannedHoursOfDepartmentOnDate(DateTime date, int departmentId)
        {
            double hoursPlanned = 0;
            List<Shift> shifts = _ctx.Shift
                .Where(f => f.ShiftStartDateTime.Date == date && f.DepartmentId == departmentId)
                .ToList();
            if(!shifts.Any())
            {
                return 0;
            }
            foreach(Shift shift in shifts)
            {
                hoursPlanned += CalculateShiftDuration(shift);
            }

            return hoursPlanned;
        }

        private static double CalculateShiftDuration(Shift shift)
        {
            return (shift.ShiftEndDateTime - shift.ShiftStartDateTime).TotalHours;
        }

        public int GetShiftCountInRange(DateTime start, DateTime end, string employeeId)
        {
            return _ctx.Shift
                .Where(s => s.EmployeeId == employeeId)
                .Where(s => s.ShiftStartDateTime > start && s.ShiftStartDateTime < end)
                .Count();
        }
    }
}
