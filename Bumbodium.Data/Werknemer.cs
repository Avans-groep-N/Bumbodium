using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bumbodium.Data
{
    public class Werknemer
    {
        [Key]
        public int WerknemerId { get; set; }

        [Required]
        [StringLength(64)]
        public string Voornaam { get; set; }

        [StringLength(16)]
        public string TussenVoegsel { get; set; }

        [Required]
        [StringLength(64)]
        public string Achternaam { get; set; }

        [Required]
        public DateTime Geboortedatum { get; set; }

        [Required]
        [StringLength(16)]
        public string Telefoonnummer { get; set; }

        [Required]
        [StringLength(64)]
        public string Email { get; set; }

        [Required]
        public DateTime DatumInDienst { get; set; }

        public DateTime DatumUitDienst { get; set; }

        [StringLength(64)]
        public string Functie { get; set; }
    }
}
