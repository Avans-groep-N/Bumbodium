namespace Bumbodium.WebApp.Models.ClockingView
{
    public class ClockingManagerViewModel
    {
        public DateTime ClockingDateTime { get; set; }

        public Dictionary<string, List<ManagerClockingItem>> ClockingDay { get; set; }

        public ClockingManagerViewModel()
        {
            ClockingDay = new Dictionary<string, List<ManagerClockingItem>>();
        }

        public void AddToClockingDays(string id, ManagerClockingItem managerClocking)
        {
            if (!ClockingDay.ContainsKey(id))
                ClockingDay[id] = new List<ManagerClockingItem>();
            ClockingDay[id].Add(managerClocking);
        }
    }
}
