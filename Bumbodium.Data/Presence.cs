﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Bumbodium.Data
{
    public class Presence
    {
        [Key]
        [Required]
        public DateTime ClockInDateTime { get; set; }

        [Required]
        public DateTime ClockOutDateTime { get; set; }

        public DateTime AlteredClockInDateTime { get; set; }
        public DateTime AlteredClockOutDateTime { get; set; }
    }
}
