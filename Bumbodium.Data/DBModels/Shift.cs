using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Bumbodium.Data.DBModels
{
    public class Shift
    {
        [Key]
        public int ShiftId { get; set; }
        public string EmployeeId { get; set; }
        public DepartmentType DepartmentId { get; set; }

        public Department Department { get; set; }
        public Employee Employee { get; set; }

        [Required]
        public DateTime ShiftStartDateTime { get; set; }
        [Required]
        public DateTime ShiftEndDateTime { get; set; }


    }
}
