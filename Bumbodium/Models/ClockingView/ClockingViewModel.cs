namespace Bumbodium.WebApp.Models.ClockingView
{
    public class ClockingViewModel
    {

        public List<ClockingDayViewModel> ClockingDays { get; set; }

        public string EmployeeName { get; set; }

        public int WeekNumber { get; set; }

        public int YearNumber { get; set; }

        public ClockingViewModel()
        {
            ClockingDays = new List<ClockingDayViewModel>();
        }

    }
}
