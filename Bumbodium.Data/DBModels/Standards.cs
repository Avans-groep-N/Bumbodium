using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bumbodium.Data.DBModels
{
    public class Standards
    {
        [Key]
        [StringLength(32)]
        public string Id { get; set; }

        [Required]
        public List<Forecast> ForecastId { get; set; }

        [Required]
        public int Value { get; set; }
        
        [StringLength(1048)]
        public string Description { get; set; }

        [Required]
        public virtual int CountryId { get; set; }
        public virtual Country Country { get; set; }
    }
}
