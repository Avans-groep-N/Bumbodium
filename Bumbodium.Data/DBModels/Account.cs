using System.ComponentModel.DataAnnotations;

namespace Bumbodium.Data.DBModels
{
    public class Account
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        public Employee Employee { get; set; }

        [StringLength(64)]
        public string Username { get; set; }

        [Required]
        [StringLength(64)]
        public string Password { get; set; }
    }
}
