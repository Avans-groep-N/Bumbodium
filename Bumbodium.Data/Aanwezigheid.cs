using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Bumbodium.Data
{
    public class Aanwezigheid
    {
        [Key]
        [Required]
        public DateTime InklokDatumTijd { get; set; }

        [Required]
        public DateTime UitklokDatumTijd { get; set; }

        [Required]
        public List<Werknemer> Werknemer { get; set; }

        public DateTime AangepasteInklokDatumTijd { get; set; }
        public DateTime AangepasteUitklokDatumTijd { get; set; }
    }
}
