
using System.ComponentModel.DataAnnotations;

namespace Bumbodium.WebApp.Models
{
    public class EmployeeCreateViewModel : EmployeeViewModel
    {
        [Required]
        public override string Password { get; set; }

        [Required]
        public override string ConfirmPassword { get; set; }
    }
}
