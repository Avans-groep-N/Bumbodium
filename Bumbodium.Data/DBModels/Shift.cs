﻿using System.ComponentModel.DataAnnotations;

namespace Bumbodium.Data.DBModels
{
    public class Shift
    {
        [Key]
        public int ShiftId { get; set; }

        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        [Required]
        public DateTime ShiftStartDateTime { get; set; }

        [Required]
        public DateTime ShiftEndDateTime { get; set; }
        public string Text
        {
            get
            {
                return EmployeeId.ToString();
            }
        }
    }
}
