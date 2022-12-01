using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Bumbodium.Data.DBModels
{
    public class BranchEmployee
    {
        public string EmployeeId { get; set; }
        public int FiliaalId { get; set; }

        public Branch Filiaal { get; set; }
        public Employee Employee { get; set; }
    }
}
