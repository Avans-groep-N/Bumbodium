namespace Bumbodium.WebApp.Models.ClockingView
{
    public class ClockingEmployeeViewModel
    {
        public string EmployeeName { get; set; }

        public DateTime FirstOfTheMonth { get; set; }

        public Dictionary<DateTime, List<EmployeeClockingItem>> ClockingDays { get; set; }

        public ClockingEmployeeViewModel()
        {
            ClockingDays = new Dictionary<DateTime, List<EmployeeClockingItem>>();
        }

        public void AddToClockingDays(DateTime date, EmployeeClockingItem managerClocking)
        {
            if (!ClockingDays.ContainsKey(date))
                ClockingDays[date] = new List<EmployeeClockingItem>();
            ClockingDays[date].Add(managerClocking);
        }
    }
}
