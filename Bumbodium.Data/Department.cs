using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bumbodium.Data
{
    public class Department
    {
        [Key]
        [Required]
        public DepartmentType Name { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        [Required]
        public List<Shift> Shifts { get; set; }

        [Required]
        public List<Forecast> ForecastId { get; set; }

        public virtual ICollection<DepartmentEmployee> PartOFEmployee { get; set; }
    }

    public enum DepartmentType
    {
        Vis,
        Groente
        //TODO
    }
}
