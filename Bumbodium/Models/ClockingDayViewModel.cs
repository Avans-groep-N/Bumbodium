namespace Bumbodium.WebApp.Models
{
    public class ClockingDayViewModel
    {

        public List<ClockingItemViewModel> ClockingItems;
        public DateTime Day;

        public ClockingDayViewModel()
        {
            ClockingItems = new List<ClockingItemViewModel>();
        }

    }
}
