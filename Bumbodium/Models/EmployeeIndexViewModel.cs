using Bumbodium.Data.DBModels;

namespace Bumbodium.WebApp.Models
{
    public class EmployeeIndexViewModel
    {
        public IEnumerable<Employee> Employees { get; set; }
        public int EmployeeCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount => EmployeeCount / 5;

        public string? NameFilter { get; set; }
        public int? AgeFilter { get; set; }
        public IEnumerable<int>? DepartmentsIdFiter { get; set; }
        public TypeStaff? TypeFilter { get; set; }
        public bool DateOutService { get; set; }
    }
}
