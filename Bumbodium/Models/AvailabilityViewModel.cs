﻿using Bumbodium.Data;
using Bumbodium.Data.DBModels;
using Bumbodium.Data.Utilities.EmployeeValidation;
using Bumbodium.WebApp.Models.Utilities.AvailabilityValidation;
using System.ComponentModel.DataAnnotations;

namespace Bumbodium.WebApp.Models
{
    public class AvailabilityViewModel : IValidatableObject
    {
        [Required]
        //[ValidateDate]
        public DateOnly Date { get; set; }
        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        [Required]
        public AvailabilityType AvailabilityType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //Db doohickery
            var _ctx = (BumbodiumContext)validationContext
                         .GetService(typeof(BumbodiumContext));
            var _availabilityRepo = new AvailabilityRepo(_ctx);

            //Verify that availability can only be added if startTime is before EndTime
            if (EndTime.CompareTo(StartTime) == -1)
                yield return new ValidationResult("End time cannot be before start time", new[] { "StartTime" });

            //Verify that user cannot have multiple availabilities on the same time and day
            var availabilities = _availabilityRepo.GetAvailabilitiesInRange(Date.ToDateTime(StartTime), Date.ToDateTime(EndTime));
            if (availabilities.Count > 0)
                yield return new ValidationResult("Cannot have multiple availabilities at the same time", new[] { "StartTime" });

            //Verify that user cannot add availability without a 2 week notice
            if (Date.Day < DateTime.Today.AddDays(14).Day)
                yield return new ValidationResult("Cannot add availability under 2 weeks in advance", new[] { "StartTime" });

            //Verify that user cannot add availability in the past
            if (DateTime.Now.CompareTo(StartTime) > 0)
                yield return new ValidationResult("Cannot add availability in the past", new[] { "StartTime" });
        }
    }
}

