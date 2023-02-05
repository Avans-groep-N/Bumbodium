using Bumbodium.Data;
using Bumbodium.Data.DBModels;
using System.ComponentModel.DataAnnotations;

namespace Bumbodium.WebApp.Models
{
    public class AvailabilityVM : IValidatableObject
    {
        [Required]
        public string EmployeeName { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public AvailabilityType Type { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //Db doohickery
            var _ctx = (BumbodiumContext)validationContext
                         .GetService(typeof(BumbodiumContext));
            var _availabilityRepo = new AvailabilityRepo(_ctx);

            //Verify that availability can only be added if startTime is before EndTime
            if (EndTime.Subtract(StartTime) < TimeSpan.Zero)
                yield return new ValidationResult("End time cannot be before start time", new[] { "StartTime" });

            //Verify that user cannot have multiple availabilities on the same time and day
            var availabilities = _availabilityRepo.GetAvailabilitiesInRange(StartTime, EndTime);
            if (availabilities.Count > 0)
                yield return new ValidationResult("Cannot have multiple availabilities at the same time", new[] { "StartTime" });

            //Verify that user cannot add availability without a 2 week notice
            if (StartTime.Date.Subtract(DateTime.Now.Date).TotalDays < 14)
                yield return new ValidationResult("Cannot add availability under 2 weeks in advance", new[] { "StartTime" });

            //Verify that user cannot add availability in the past
            if (DateTime.Now.CompareTo(StartTime) > 0)
                yield return new ValidationResult("Cannot add availability in the past", new[] { "StartTime" });
        }
    }
}
