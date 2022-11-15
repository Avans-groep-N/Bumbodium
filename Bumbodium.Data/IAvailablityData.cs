namespace Bumbodium.Data
{
    public interface IAvailablityData
    {
        Task<List<Availability>> GetAvailabilities();
        Task InsertAvailablity(Availability availability);
    }
}