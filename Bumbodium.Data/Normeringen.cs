﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bumbodium.Data
{
    public class Normeringen
    {
        [StringLength(1048)]
        public string Beschrijving { get; set; }

        public int Waarde { get; set; }
    }
}
