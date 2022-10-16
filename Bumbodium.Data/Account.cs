﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Bumbodium.Data
{
    public class Account
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        public Employee Employee { get; set; }

        [StringLength(64)]
        public string Email { get; set; }

        [Required]
        [StringLength(64)]
        public string Password { get; set; }
        
        [Required]
        public TypeMedeweker Type { get; set; }
    }

    public enum TypeMedeweker
    {
        Manager,
        Werknemer
    }
}
