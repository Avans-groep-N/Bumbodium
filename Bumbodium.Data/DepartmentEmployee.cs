using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Bumbodium.Data
{
    public class DepartmentEmployee
    {
        public int EmployeeId { get; set; }
        public DepartmentType DepartmentId { get; set; }

        public Department Department { get; set; }
        public Employee Employee { get; set; }

        public WorkFunction WorkFunction { get; set; }
    }

    public enum WorkFunction
    {
        Fresh,
        stockClerk,
        TeamLeader,
        Butcher
    }
}
