using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bumbodium.Data
{
    public class Filiaal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string Stad { get; set; }
        [Required]
        [StringLength(64)]
        public string Straat { get; set; }
        [Required]
        [StringLength(64)]
        public string Huisnummer { get; set; }
        [Required]
        [StringLength(64)]
        public string Land { get; set; }
    }
}
