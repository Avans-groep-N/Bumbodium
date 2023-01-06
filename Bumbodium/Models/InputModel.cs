using Bumbodium.Data.DBModels.EmployeeValidation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Bumbodium.Data.DBModels;

namespace Bumbodium.WebApp.Models
{
    public class InputModel : LoginModel
    {

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