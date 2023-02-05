namespace Bumbodium.WebApp.Models.ClockingView
{
    public class EmployeeClockingItem
    {
        public DateTime ClockStartTime { get; set; }
        public DateTime ClockEndTime { get; set; }
        public bool IsSick { get; set; }
        public bool IsStartChanged { get; set; }
        public bool IsEndChanged { get; set; }
    }
}
