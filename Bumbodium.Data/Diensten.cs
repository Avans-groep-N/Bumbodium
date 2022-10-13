using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Bumbodium.Data
{
    public class Diensten
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public List<Werknemer> Werknemer { get; set; }
        [Required]
        public List<Afdeling> AfdelingNaam { get; set; }
        [Required]
        public DateTime DienstBeginDatumTijd { get; set; }
        [Required]
        public DateTime DienstEindDatumTijd { get; set; }
    }
}
