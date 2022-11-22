using Bumbodium.Data.DBModels;

namespace Bumbodium.Data.Interfaces
{
    public interface IAvailablityRepo
    {
        Task<List<Availability>> GetAvailabilities();
        Task<List<Availability>> GetAvailabilitiesInRange(DateTime start, DateTime end);
        Task InsertAvailability(Availability availability);
        Task DeleteAvailability(Availability availability);
        Task UpdateAvailability(Availability availability);
    }
}