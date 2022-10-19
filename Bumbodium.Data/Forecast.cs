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

        [Required]
        public int AmountExpectedEmployees { get; set; }
        
        [Required]
        public int AmountExpectedCustomers { get; set; }

    }
}
