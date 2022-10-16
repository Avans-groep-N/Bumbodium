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
        public AfdelingType Name { get; set; }

        [StringLength(256)]
        public string Description { get; set; }


        public virtual ICollection<DepartmentEmployee> PartOFEmployee { get; set; }
    }

    public enum AfdelingType
    {
        Vis,
        Groente
        //TODO
    }
}
