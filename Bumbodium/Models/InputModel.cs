using Bumbodium.Data.Utilities.EmployeeValidation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Bumbodium.Data.DBModels;

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
        [ValidateAgeAttribute()]
        public DateTime Birthday { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [ValidateDateInServiceAttribute()]
        public DateTime DateInService { get; set; }
        public Bumbodium.Data.DBModels.TypeStaff TypeStaff { get; set; }

        //List for displaying departments
        public IEnumerable<Department>? DepartmentList { get; set; }
        [Required]
        public List<int> ActiveDepartmentIds { get; set; }
    }
}