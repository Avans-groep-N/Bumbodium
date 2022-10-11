using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bumbodium.Data
{
    public class Aanwezigheid
    {
        [Key]
        [Required]
        public DateTime InklokDatumTijd { get; set; }
        [Key]
        [Required]
        public DateTime UitklokDatumTijd { get; set; }
        [Key]
        [Required]
        public int WerkNemerID { get; set; }

        public DateTime AangepasteInklokDatumTijd { get; set; }
        public DateTime AangepasteUitklokDatumTijd { get; set; }
    }
}
