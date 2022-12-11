using Bumbodium.Data.Utilities;
using Bumbodium.Data.Utilities.EmployeeValidation;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bumbodium.Data.DBModels
{
    public class Employee
    {
        [Key]
        public string EmployeeID { get; set; }

        [Required]
        [StringLength(64)]
        public string FirstName { get; set; }

        [StringLength(16)]
        public string? MiddleName { get; set; }

        [Required]
        [StringLength(64)]
        public string LastName { get; set; }

        [Required]
        [ValidateAgeAttribute(15, 67)]
        public DateTime Birthdate { get; set; }

        [Required]
        [StringLength(16)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(64)]
        public string Email { get; set; }

        [Required]
        [ValidateDateInService(2000)]
        public DateTime DateInService { get; set; }

        public DateTime? DateOutService { get; set; }

        [Required]
        public TypeStaff Type { get; set; }

        public List<Availability> Availability { get; set; }
        public List<Presence> Presence { get; set; }
        [ForeignKey("EmployeeID")]
        public IdentityUser Account { get; set; }
        public virtual ICollection<Shift> Shifts { get; set; }
        public virtual ICollection<BranchEmployee> PartOFFiliaal { get; set; }
        public virtual ICollection<DepartmentEmployee> PartOFDepartment { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + MiddleName + " " + LastName;
            }
        }
        public int Age
        {
            get
            {
                int age;
                age = DateTime.Now.Year - Birthdate.Year;

                if (age > 0)
                {
                    age -= Convert.ToInt32(DateTime.Now.Date < Birthdate.Date.AddYears(age));
                }
                else
                {
                    age = 0;
                }

                return age;
            }
        }

    }

    public enum TypeStaff
    {
        Manager,
        Employee
    }
}
