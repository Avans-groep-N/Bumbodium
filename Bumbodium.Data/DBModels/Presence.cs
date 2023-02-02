using System.ComponentModel.DataAnnotations;

namespace Bumbodium.Data.DBModels
{
    public class Presence
    {
        [Key]
        public int PresenceId { get; set; }
        
        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [Required]
        public DateTime ClockInDateTime { get; set; }

        [Required]
        public DateTime? ClockOutDateTime { get; set; }

        public DateTime? AlteredClockInDateTime { get; set; }
        public DateTime? AlteredClockOutDateTime { get; set; }

        public bool IsSick { get; set; }
    }
}
