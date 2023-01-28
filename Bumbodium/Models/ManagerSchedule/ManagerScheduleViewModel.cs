using Bumbodium.Data.DBModels;

namespace Bumbodium.WebApp.Models.ManagerSchedule
{
    public class ManagerScheduleViewModel
    {
        // EmployeeSelectionList
        public List<Employee>? AvailableEmployees { get; set; }
        public string? SelectedEmployeeId { get; set; }
        public DepartmentType SelectedDepartment { get; set; }
        public DateTime SelectedDate { get; set; }
        public DateTime SelectedStartTime { get; set; }
        public DateTime SelectedEndTime { get; set; }

        // Shifts list
        public List<Shift> Shifts;
        public DateTime Day;
        public TimeOnly OpenTime = new(08, 00, 00);
        public TimeOnly ClosingTime = new(22, 00, 00);

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
