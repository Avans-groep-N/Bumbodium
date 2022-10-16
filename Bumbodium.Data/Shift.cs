using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Bumbodium.Data
{
    public class Shift
    {
        [Key]
        public int ShiftId { get; set; }
        [Required]
        public List<Employee> EmploeeID{ get; set; }
        [Required]
        public List<Department> DepartmentName { get; set; }
        [Required]
        public DateTime ShiftStartDateTime { get; set; }
        [Required]
        public DateTime ShiftEndDateTime { get; set; }
    }
}
