using Bumbodium.Data.DBModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bumbodium.WebApp.Models
{
    public class ForecastWeekViewModel
    {
        public ForecastViewModel[] DaysOfTheWeek { get; set; } = new ForecastViewModel[7];
        public int DepartmentId { get; set; }
    }

    public class ForecastViewModel : IValidatableObject
    {
        [Key]
        public DateTime Date { get; set; }

        public int DepartmentId { get; set; }

        [Required]
        public int AmountExpectedEmployees { get; set; }

        [Required]
        public int AmountExpectedHours { get; set; }

        [Required]
        public int AmountExpectedCustomers { get; set; }

        [Required]
        public int AmountExpectedColis { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (false)
            {
                yield return new ValidationResult("peepee", new[] {"lol"});
            }
        }
    }
}
