using System.ComponentModel.DataAnnotations;

namespace Bumbodium.WebApp.Models
{
    public class AvailabilityViewModel : IValidatableObject
    {
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public TimeOnly StartTime { get; set; }

        [Required] 
        public TimeOnly EndTime { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(StartTime > EndTime)
            {
                //TODO: add appropriate message
                yield return new ValidationResult("");
            }
            if(StartTime == EndTime)
            {
                yield return new ValidationResult("");
            }
            //TODO: add minimum amount of time (maybe)
        }
    }
}
