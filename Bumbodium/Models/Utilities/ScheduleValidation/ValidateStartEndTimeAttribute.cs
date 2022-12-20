using System.ComponentModel.DataAnnotations;

namespace Bumbodium.WebApp.Models.Utilities.ScheduleValidation
{
    public class ValidateStartEndTimeAttribute : ValidationAttribute
    {
        private readonly DateTime _startTime;
        private readonly DateTime _endTime;

        public ValidateStartEndTimeAttribute(DateTime shift)
        {
            _startTime = shift.StartTime;
            _endTime = shift.EndTime;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            //nullcheck
            if (value == null)
                return new ValidationResult("Fields cannot be empty", new[] { "StartTime" });

            //If end - start < 0
            if(_endTime.Subtract(_startTime) < TimeSpan.Zero)
                return new ValidationResult("End time cannot be before start time", new[] { "StartTime" });

            //If no errors were returned, return null
            return null;
        }
    }
}
