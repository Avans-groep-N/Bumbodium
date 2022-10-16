using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bumbodium.Data
{
    public class Filiaal
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(64)]
        public string City { get; set; }
        [Required]
        [StringLength(64)]
        public string Streed { get; set; }
        [Required]
        [StringLength(64)]
        public string PostalCode { get; set; }
        [Required]
        [StringLength(64)]
        public string HomeNumber { get; set; }
        [Required]
        [StringLength(64)]
        public string Country { get; set; }
    }
}
