using System.ComponentModel.DataAnnotations;

namespace Bumbodium.WebApp.Models.ClockingView
{
    public class ManagerClockingItem : IValidatableObject
    {
        public int PresenceId { get; set; }
        public DateTime Date { get; set; }
        public DateTime ClockStartTime { get; set; }
        public DateTime? ClockEndTime { get; set; }
        public DateTime? ScheduleStartTime { get; set; }
        public DateTime? ScheduleEndTime { get; set; }
        public bool IsSick { get; set; }

        public string Name { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ClockStartTime >= ClockEndTime)
                yield return new ValidationResult("Start tijd kan niet na of gelijk aan eind tijd zijn.", new[] { nameof(ClockStartTime) });
        }
    }
}
