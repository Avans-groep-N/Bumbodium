namespace Bumbodium.Data
{
    public interface IAvailablityData
    {
        Task<List<Availability>> GetAvailabilities();
        Task<List<Availability>> GetAvailabilitiesInRange(DateTime start, DateTime end);
        Task InsertAvailability(Availability availability);
        Task DeleteAvailability(Availability availability);
        Task UpdateAvailability(Availability availability);
    }
}