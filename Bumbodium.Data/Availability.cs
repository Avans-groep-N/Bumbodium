using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Bumbodium.Data
{
    public class Availability
    {
        [Key]
        public int AvailabilityId { get; set; }
        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        [Required]
        public BeschikbaarheidType Type { get; set; }

    }

    public enum BeschikbaarheidType
    {
        Schoolhours,
        Leave
    }
}
