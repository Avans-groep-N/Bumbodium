﻿namespace Bumbodium.Data.DBModels
{
    public class BranchEmployee
    {
        public string EmployeeId { get; set; }
        public int FiliaalId { get; set; }

        public Branch Filiaal { get; set; }
        public Employee Employee { get; set; }
    }
}
