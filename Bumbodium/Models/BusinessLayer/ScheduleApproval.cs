using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Bumbodium.WebApp.Models.BusinessLayer
{
    public class ScheduleApproval : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if((value as string).Contains("pain peko"))
            {
                return new ValidationResult($"suck my dick");
            }
            return ValidationResult.Success;

        }


    }
}
