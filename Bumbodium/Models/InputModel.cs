using System.ComponentModel.DataAnnotations;

namespace Bumbodium.WebApp.Models
{
    public class InputModel : LoginModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
