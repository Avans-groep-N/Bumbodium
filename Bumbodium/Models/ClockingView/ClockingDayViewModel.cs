namespace Bumbodium.WebApp.Models.ClockingView
{
    public class ClockingDayViewModel
    {

        public List<MedewerkerClockingItem> EmployeeClocking;

        public List<ManagerClockingItem> ManagerClocking;
        public DateTime Day;

        public ClockingDayViewModel()
        {
            EmployeeClocking = new List<MedewerkerClockingItem>();
            ManagerClocking = new List<ManagerClockingItem>();
        }

    }
}
