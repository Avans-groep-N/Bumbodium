using Bumbodium.Data.Utilities.EmployeeValidation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Bumbodium.WebApp.Models
{
    public class InputModel : LoginModel
    {

        // TODO: working static variables
/*        private readonly static int _minAge = 15;
        private readonly static int _maxAge = 67;
        private readonly static int _minStartingYear = 2000;*/

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]

        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [ValidateAgeAttribute(15, 67)]
        public DateTime Birthday { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [ValidateDateInServiceAttribute(2000)]
        public DateTime DateInService { get; set; }

    }
}