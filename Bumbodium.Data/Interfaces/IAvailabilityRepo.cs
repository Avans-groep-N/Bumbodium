using Bumbodium.Data.DBModels;

namespace Bumbodium.Data.Interfaces
{
    public interface IAvailabilityRepo
    {
        List<Availability> GetAvailabilities();
        List<Availability> GetAvailabilitiesInRange(DateTime start, DateTime end);
        List<Availability> GetAvailabilitiesInRange(DateTime start, DateTime end, string userId);
        void InsertAvailability(Availability availability);
        void DeleteAvailability(Availability availability);
        void UpdateAvailability(Availability availability);
        IQueryable<Employee> GetAvailableEmployees(int departmentId, DateTime startTime, DateTime endTime);
    }
}