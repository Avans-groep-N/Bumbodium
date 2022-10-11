using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bumbodium.Data
{
    public class Diensten
    {
        [Key]
        [Required]
        public List<Werknemer> WerknemerId { get; set; }
        [Key]
        [Required]
        public List<Afdeling> AfdelingNaam { get; set; }
        [Key]
        [Required]
        public DateTime DienstBeginDatumTijd { get; set; }
        [Key]
        [Required]
        public DateTime DienstEindDatumTijd { get; set; }
    }
}
