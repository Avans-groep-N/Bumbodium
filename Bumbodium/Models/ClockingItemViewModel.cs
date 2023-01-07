using System.ComponentModel.DataAnnotations;

namespace Bumbodium.WebApp.Models
{
    public class ClockingItemViewModel : IValidatableObject
    {

        public DateTime ClockStartTime { get; set; }
        public DateTime? ClockEndTime { get; set; }
        public bool IsOnGoing { get; set; }
        public bool IsChanged { get; set; }
        public DateTime ScheduleStartTime { get; set; }
        public DateTime ScheduleEndTime { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //Nullcheck
            if (ClockEndTime is not null)
            {
                //Verify that a manager cannot put in invalid times
                if (ClockStartTime.Subtract(ClockEndTime.Value) > TimeSpan.Zero)
                    yield return new ValidationResult("End time cannot be before start time", new[] { "ClockStartTime" });
            }
        }
    }
}
