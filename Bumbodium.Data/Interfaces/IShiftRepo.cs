using Bumbodium.Data.DBModels;

namespace Bumbodium.Data.Interfaces
{
    public interface IShiftRepo
    {
        Task<List<Shift>> GetShiftsInRange(DateTime start, DateTime end);
        Task InsertShift(Shift Shift);
        Task DeleteShift(Shift Shift);
        Task UpdateShift(Shift Shift);
        Task<List<Employee>> GetEmployeesInRange(int departmentId, string? filter, int offset, int top);
        Task<int> GetEmployeeCount(int departmentId, string? filter);
    }
}
