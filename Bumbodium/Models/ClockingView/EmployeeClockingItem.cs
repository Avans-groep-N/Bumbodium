namespace Bumbodium.WebApp.Models.ClockingView
{
    public class EmployeeClockingItem
    {
        public DateTime? ClockStartTime { get; set; }
        public DateTime? ClockEndTime { get; set; }
        public bool IsOnGoing { get; set; }
        public bool IsChanged { get; set; }
    }
}
