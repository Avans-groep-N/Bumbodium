using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bumbodium.Data
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }

        [Required]
        [StringLength(64)]
        public string FirstName { get; set; }

        [StringLength(16)]
        public string ExtraName { get; set; }

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

        public DateTime DateOutService { get; set; }

        [StringLength(64)]
        public string WorkFunction { get; set; }

        public List<Availability> Availability { get; set; }
        public List<Presence> Presence { get; set; }
        public List<Shift> ShiftId { get; set; }
        public Account Account { get; set; }
        public virtual ICollection<FiliaalEmployee> PartOFFiliaal { get; set; }
        public virtual ICollection<DepartmentEmployee> PartOFDepartment { get; set; }
    }
}
