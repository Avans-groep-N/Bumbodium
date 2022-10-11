using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bumbodium.Data
{
    public class Prognose
    {
        public DateTime Datum { get; set; }

        public int AantalVerwachtteWerknemers { get; set; }
        public int AantalVerwachtteKlanten { get; set; }

        public List<Afdeling> Afdelingen { get; set; }

    }
}
