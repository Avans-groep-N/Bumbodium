namespace Bumbodium.WebApp.Models
{
    public class ClockingItemViewModel
    {

        public DateTime? ClockStartTime { get; set; }
        public DateTime? ClockEndTime { get; set; }
        public bool IsOnGoing { get; set; }
        public bool IsChanged { get; set; }
        public DateTime? ScheduleStartTime { get; set; }
        public DateTime? ScheduleEndTime { get; set; }

    }
}
