using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bumbodium.Data
{
    public class Prognose
    {
        public int Id { get; set; }

        public DateTime Datum { get; set; }

        [Required]
        public int AantalVerwachtteWerknemers { get; set; }
        
        [Required]
        public int AantalVerwachtteKlanten { get; set; }

        [Required]
        public List<Afdeling> Afdelingen { get; set; }

    }
}
