using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bumbodium.Data
{
    public class Beschikbaarheid
    {
        [Key]
        [Required]
        public List<Werknemer> Werknemer { get; set; }

        [Key]
        [Required]
        public DateTime BeginDatumTijd { get; set; }

        [Key]
        [Required]
        public DateTime EindDatumTijd { get; set; }

        [Required]
        public BeschikbaarheidType Type { get; set; }

    }

    public enum BeschikbaarheidType
    {
        //TODO
    }
}
