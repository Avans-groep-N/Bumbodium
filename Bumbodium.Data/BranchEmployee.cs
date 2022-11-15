using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Bumbodium.Data
{
    public class BranchEmployee
    {
        public int EmployeeId { get; set; }
        public int BranchId { get; set; }

        public Branch Branch { get; set; } 
        public Employee Employee { get; set; }
    }
}
