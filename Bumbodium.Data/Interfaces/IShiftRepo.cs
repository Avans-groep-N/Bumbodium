using Bumbodium.Data.DBModels;

namespace Bumbodium.Data.Interfaces
{
    public interface IShiftRepo
    {
        List<Shift> GetShiftsInRange(DateTime start, DateTime end);
        List<Shift> GetShiftsInRange(DateTime start, DateTime end, int departmentId);
        void InsertShift(Shift Shift);
        void DeleteShift(Shift Shift);
        void DeleteShift(int shiftId);
        void UpdateShift(Shift Shift);
        List<Employee> GetEmployeesInRange(int departmentId, string? filter, int offset, int top);
        int GetEmployeeCount(int departmentId, string? filter);
        bool ShiftExistsInTime(DateTime start, DateTime end, string employeeId);
        double GetPlannedHoursOfDepartmentOnDate(DateTime date, int departmentId);
    }
}
