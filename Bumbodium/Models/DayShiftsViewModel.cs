namespace Bumbodium.WebApp.Models
{
    public class DayShiftsViewModel
    {

        public List<ShiftVM> Planning;
        public DateTime Day;
        public DateTime OpenTime;
        public DateTime ClosingTime;

        public DayShiftsViewModel()
        {
            Planning = new List<ShiftVM>();
        }

    }
}
