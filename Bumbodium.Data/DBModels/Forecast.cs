using Bumbodium.Data.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bumbodium.Data
{
    public class Forecast
    {
        [Key]
        public DateTime Date { get; set; }

        public DepartmentType DepartmentId { get; set; }
        //public Department Department { get; set; }

        [Required]
        public int AmountExpectedEmployees { get; set; }
        
        [Required]
        public int AmountExpectedCustomers { get; set; }
        
        [Required]
        public int AmountExpectedColis { get; set; }

    }
}
