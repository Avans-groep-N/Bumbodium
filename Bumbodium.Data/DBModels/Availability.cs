﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bumbodium.Data.DBModels
{
    public class Availability
    {
        [Key]
        public int AvailabilityId { get; set; }

        public string EmployeeId { get; set; }

        public Employee Employee { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        [Required]
        public AvailabilityType Type { get; set; }

        [Required]
        public bool IsConfirmed { get; set; } = false;

        [NotMapped]
        public string Text
        {
            get
            {
                return StartDateTime.ToShortTimeString() + "-" + EndDateTime.ToShortTimeString();
            }
        }
    }

    public enum AvailabilityType
    {
        Schoolhours,
        Leave,
        Holidays
    }
}
