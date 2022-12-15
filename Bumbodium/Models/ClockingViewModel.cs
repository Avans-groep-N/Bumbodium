namespace Bumbodium.WebApp.Models
{
    public class ClockingViewModel
    {

        public List<ClockingItemViewModel> ClockingItems { get; set; }
        public ClockingViewModel()
        {
            ClockingItems = new List<ClockingItemViewModel>();
        }

        public string EmployeeName { get; set; }

        public int WeekNumber { get; set; }


    }
}
