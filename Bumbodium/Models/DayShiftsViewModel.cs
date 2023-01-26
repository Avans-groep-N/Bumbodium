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
            Day = new DateTime(2023, 01, 26);
            OpenTime = new DateTime(2023, 01, 26, 08, 00, 00);
            ClosingTime = new DateTime(2023, 01, 26, 22, 00, 00);
            Planning = new List<ShiftVM>
            {
                new ShiftVM() { EmployeeId = "1", StartTime = new DateTime(2023, 01, 26, 10, 00, 00), EndTime = new DateTime(2023, 01, 26, 13, 00, 00) },
                new ShiftVM() { EmployeeId = "2", StartTime = new DateTime(2023, 01, 26, 12, 30, 00), EndTime = new DateTime(2023, 01, 26, 18, 00, 00) },
                new ShiftVM() { EmployeeId = "3", StartTime = new DateTime(2023, 01, 26, 12, 30, 00), EndTime = new DateTime(2023, 01, 26, 18, 00, 00) },
                new ShiftVM() { EmployeeId = "4", StartTime = new DateTime(2023, 01, 26, 18, 00, 00), EndTime = new DateTime(2023, 01, 26, 21, 30, 00) },
            };
        }

        public int GetEmptyStart(DateTime input)
        {
            int empty = input.Hour - OpenTime.Hour;
            empty = empty * 2;

            int halfempty;
            if (input.Minute <= 15)
            {
                halfempty = 0;
            }
            else if (input.Minute < 45)
            {
                halfempty = 1;
            }
            else
            {
                halfempty = 2;
            }

            empty = empty + halfempty;
            return empty;
        }

        public int GetWorkHours(DateTime start, DateTime end)
        {
            TimeSpan difference = end - start;
            int worked = difference.Hours * 2;

            int halfworked;
            if (difference.Minutes <= 15)
            {
                halfworked = 0;
            }
            else if (difference.Minutes < 45)
            {
                halfworked = 1;
            }
            else
            {
                halfworked = 2;
            }

            worked = worked + halfworked;
            return worked;
        }
    }
}
