using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bumbodium.Data
{
    public class Account
    {
        [Key]
        [StringLength(64)]
        public string Email { get; set; }

        [Key]
        [StringLength(64)]
        public string Wachtwoord { get; set; }
        
        [Required]
        public TypeMedeweker Type { get; set; }
    }

    public enum TypeMedeweker
    {
        Manager,
        Werknemer
    }
}
