﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bumbodium.Data
{
    public class Standards
    {
        [Key]   
        [StringLength(1048)]
        public string Description { get; set; }

        [Required]
        public List<Forecast> ForecastId { get; set; }

        [Required]
        public int Value { get; set; }
    }
}
