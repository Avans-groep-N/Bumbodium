using Bumbodium.Data.DBModels;

namespace Bumbodium.Data.Interfaces
{
    public interface IShiftRepo
    {
        Task<List<Shift>> GetShiftsInRange(DateTime start, DateTime end);
        Task InsertShift(Shift Shift);
        Task DeleteShift(Shift Shift);
        Task UpdateShift(Shift Shift);
    }
}
