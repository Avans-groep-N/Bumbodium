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
        public int Id { get; set; }
        [Required]
        public List<Employee> EmployeeID { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        [Required]
        public BeschikbaarheidType Type { get; set; }

    }

    public enum BeschikbaarheidType
    {
        //TODO
    }
}
