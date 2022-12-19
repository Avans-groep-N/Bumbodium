using Bumbodium.Data.DBModels;

namespace Bumbodium.Data.Interfaces
{
    public interface IShiftRepo
    {
        List<Shift> GetShiftsInRange(DateTime start, DateTime end);
        void InsertShift(Shift Shift);
        void DeleteShift(Shift Shift);
        void UpdateShift(Shift Shift);
        List<Employee> GetEmployeesInRange(int departmentId, string? filter, int offset, int top);
        int GetEmployeeCount(int departmentId, string? filter);
    }
}
