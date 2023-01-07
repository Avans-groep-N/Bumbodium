using Bumbodium.Data.DBModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Bumbodium.WebApp.Models
{
    public class AvailabilityListViewModel
    {
        [Required]
        [BindProperty]
        public List<AvailabilityViewModel> Availabilities { get; set; }
        public string[] Opdions = new[] { "Accepted", "Denied" };

        [BindProperty]
        public string Gender { get; set; }
        public string[] Genders = new[] { "Male", "Female", "Unspecified" };
    }

    public class AvailabilityViewModel
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
