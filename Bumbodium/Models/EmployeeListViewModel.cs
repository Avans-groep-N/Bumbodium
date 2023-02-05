using Bumbodium.Data.DBModels;

namespace Bumbodium.WebApp.Models
{
    public class EmployeeListViewModel
    {
        public List<Employee> Employees { get; set; }
        public int EmployeeCount { get; set; }
        public int CurrentPage { get; set; }
        public int EmployeesPerPage { get; set; }
        public int PageCount
        {
            get
            {
                int result = EmployeeCount / EmployeesPerPage;
                if (EmployeeCount % EmployeesPerPage != 0)
                {
                    result++;
                }
                return result;
            }
        }


        public string? NameFilter { get; set; }
        public int DepartmentFilter { get; set; }
        public bool ShowInactive { get; set; }

        public EmployeeListViewModel()
        {
            Employees = new List<Employee>();
        }
    }
}
