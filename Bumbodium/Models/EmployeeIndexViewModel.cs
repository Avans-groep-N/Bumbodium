using Bumbodium.Data.DBModels;

namespace Bumbodium.WebApp.Models
{
    public class EmployeeIndexViewModel
    {
        public IEnumerable<Employee> Employees { get; set; }
        public int EmployeeCount { get; set; }
        public int CurrentPage { get; set; }
        public int EmployeesPerPage { get; set; }
        public int PageCount => EmployeeCount / EmployeesPerPage;

        public string? NameFilter { get; set; }
        public int? DepartmentIdFiter { get; set; }
        public bool ShowInactive { get; set; }
    }
}
