using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bumbodium.Data
{
    public class Afdeling
    {
        [Required]
        public AfdelingType Naam { get; set; }

        [StringLength(256)]
        public string Beschrijving { get; set; }
    }

    public enum AfdelingType
    {
        Vis,
        Groente
        //TODO
    }
}
