using System.ComponentModel.DataAnnotations;

namespace Bumbodium.WebApp.Models.Utilities.AvailabilityValidation
{
    public class ValidateDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
                ValidationContext validationContext)
        {
            DateTime newvalue = Convert.ToDateTime(value.ToString());
            if(newvalue < DateTime.Today.AddDays(14))
            {
                return new ValidationResult("Cannot add availability under 2 weeks in advance");
            }
            return ValidationResult.Success;

        }
    }
}
