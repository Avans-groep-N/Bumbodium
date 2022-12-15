namespace Bumbodium.WebApp.Models
{
    public class ClockingViewModel
    {

        public List<ClockingDayViewModel> ClockingDays { get; set; }
        public ClockingViewModel()
        {
            ClockingDays = new List<ClockingDayViewModel>();
        }

        public string EmployeeName { get; set; }

        public int WeekNumber { get; set; }


    }
}
