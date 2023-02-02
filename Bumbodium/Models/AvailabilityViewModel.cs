using Bumbodium.Data;
using Bumbodium.Data.DBModels;
using Bumbodium.WebApp.Models.Utilities.AvailabilityValidation;
﻿using Bumbodium.Data.DBModels;
using Microsoft.AspNetCore.Mvc;
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
                yield return new ValidationResult("Eind tijd kan niet voor de begin tijd", new[] { "StartTime" });

            

            //Verify that user cannot add availability without a 2 week notice
            if (Date.Day < DateTime.Today.AddDays(14).Day)
                yield return new ValidationResult("Kan beschikbaarheid niet toevoegen minder dan 2 weken van tevoren", new[] { "StartTime" });

            //Verify that user cannot add availability in the past
            if (Date < DateOnly.FromDateTime(DateTime.Now))
                yield return new ValidationResult("Kan beschikbaarheid niet in het verleden toevoegen", new[] { "StartTime" });
        }
    }
}

    public class AvailabilityListViewModel
    {
        [Required]
        [BindProperty]
        public List<AvailabilityViewModel> Availabilities { get; set; }
        public string[] Options = new[] { "Accepted", "Denied" };
    }

    public class AvailabilityAccodationViewModel
    {
        public int Id { get; set; }
        public Employee Employee { get; set; }
        public string EmployeeId { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        [Required]
        public AvailabilityType Type { get; set; }

        [Required]
        public bool IsConfirmed { get; set; } = false;
    }
}
