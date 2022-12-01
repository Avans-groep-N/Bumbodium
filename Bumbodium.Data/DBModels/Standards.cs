﻿using System.ComponentModel.DataAnnotations;

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
        public Country Country { get; set; }
    }

    public enum Country
    {
        Netherlands
    }
}
