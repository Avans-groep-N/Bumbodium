using Bumbodium.Data.DBModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bumbodium.WebApp.Models
{
    public class ForecastWeekViewModel
    {
        public ForecastViewModel[] DaysOfTheWeek { get; set; } = new ForecastViewModel[7];
    }

    public class ForecastViewModel : IValidatableObject
    {
        [Key]
        [Required]
        public DateTime? Date { get; set; }

        public int? DepartmentId { get; set; }

        public int? AmountExpectedEmployees { get; set; }

        public int? AmountExpectedHours { get; set; }

        [Required]
        public int? AmountExpectedCustomers { get; set; }

        [Required]
        public int? AmountExpectedColis { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //Validate that there are no negative numbers present
            if (AmountExpectedColis < 0)
            {
                yield return new ValidationResult("Negative numbers cannot be added", new[] { "AmountExpectedColis" });
            }
            if (AmountExpectedCustomers < 0)
            {
                yield return new ValidationResult("Negative numbers cannot be added", new[] { "AmountExpectedCustomers" });
            }

            //Validate date isn't in the past
            if (DateTime.Now.CompareTo(Date) > 0)
                yield return new ValidationResult("Date cannot be in the past", new[] { "Date" });
        }
    }
}
