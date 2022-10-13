using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Bumbodium.Data
{
    public class Beschikbaarheid
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public List<Werknemer> Werknemer { get; set; }

        [Required]
        public DateTime BeginDatumTijd { get; set; }

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
