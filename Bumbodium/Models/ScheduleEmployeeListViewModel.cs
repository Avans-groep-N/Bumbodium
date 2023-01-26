using Bumbodium.Data.DBModels;
namespace Bumbodium.WebApp.Models
{
    public class ScheduleEmployeeListViewModel
    {
        public List<Employee> AvailableEmployees { get; set; }
        public string SelectedEmployeeId { get; set; }
        public DepartmentType SelectedDepartment { get; set; }
        public DateTime SelectedStartTime { get; set; }
        public DateTime SelectedEndTime { get; set; }

    }
}