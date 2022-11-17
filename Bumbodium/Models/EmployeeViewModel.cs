using Bumbodium.Data;
using System.ComponentModel.DataAnnotations;

namespace Bumbodium.WebApp.Models
{
    public class EmployeeViewModel
    {
        [Key]
        public int EmployeeID { get; set; }

        [Required]
        [StringLength(64)]
        public string FirstName { get; set; }

        [StringLength(16)]
        public string? MiddleName { get; set; }

        [Required]
        [StringLength(64)]
        public string LastName { get; set; }

        [Required]
        public DateTime Birthdate { get; set; }

        [Required]
        [StringLength(16)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(64)]
        public string Email { get; set; }

        [Required]
        public DateTime DateInService { get; set; }

        public DateTime? DateOutService { get; set; }

        [Required]
        public TypeStaff Type { get; set; }

    }

    public enum TypeStaff
    {
        Manager,
        Employee
    }
}

