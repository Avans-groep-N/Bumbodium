namespace Bumbodium.WebApp.Models.ClockingView
{
    public class ClockingDayViewModel
    {

        public List<EmployeeClockingItem> EmployeeClocking;

        public List<ManagerClockingItem> ManagerClocking;
        public DateTime Day;

        public ClockingDayViewModel()
        {
            EmployeeClocking = new List<EmployeeClockingItem>();
            ManagerClocking = new List<ManagerClockingItem>();
        }

    }
}
