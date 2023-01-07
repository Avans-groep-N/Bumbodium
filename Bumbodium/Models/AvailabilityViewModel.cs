using Bumbodium.Data.DBModels;
using System.ComponentModel.DataAnnotations;

namespace Bumbodium.WebApp.Models
{
    public class AvailabilityViewModel 
    {
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        //TODO: add custom validation
        public TimeOnly EndTime { get; set; }

        [Required]
        public AvailabilityType AvailabilityType { get; set; }
    }
}
