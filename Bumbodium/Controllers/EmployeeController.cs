using Bumbodium.Data;
using Bumbodium.Data.DBModels;
using Bumbodium.WebApp.Models;
using Bumbodium.WebApp.Models.ExcelExport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Bumbodium.WebApp.Controllers
{
    [Authorize(Roles = "Manager")]
    public class EmployeeController : Controller
    {
        private readonly EmployeeRepo _employeeRepo;

        public EmployeeController(EmployeeRepo employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        public IActionResult Index()
        {
            IQueryable<Employee> employees = _employeeRepo.GetEmployees().Include(e => e.PartOFDepartment).ThenInclude(pod => pod.Department).OrderBy(e => e.FirstName);
            int employeesPerPage = 3;
            return View(new EmployeeIndexViewModel()
            {
                CurrentPage = 1,
                EmployeesPerPage = employeesPerPage,
                Employees = employees.Take(employeesPerPage),
                EmployeeCount = employees.Count()
            }) ;
        }

        [HttpPost]
        public IActionResult Index(EmployeeIndexViewModel viewModel)
        {
            IQueryable<Employee> employees = _employeeRepo.GetEmployees()
                .Include(e => e.PartOFDepartment).ThenInclude(pod => pod.Department)
                .Where(e => e.DateOutService.HasValue == viewModel.ShowInactive);
            if(!string.IsNullOrEmpty(viewModel.NameFilter))
            {
                employees = employees.Where(e => (e.FirstName + " " + e.MiddleName + " " + e.LastName).ToLower().Contains(viewModel.NameFilter.ToLower()));
            }
            if (viewModel.DepartmentIdFiter > 0)
            {
                employees = employees.Where(e => e.PartOFDepartment.Any(pod => pod.DepartmentId == viewModel.DepartmentIdFiter));
            }
            viewModel.EmployeeCount = employees.Count();
            viewModel.Employees = employees.OrderBy(e => e.FirstName)
                .Skip(viewModel.EmployeesPerPage * (viewModel.CurrentPage - 1))
                .Take(viewModel.EmployeesPerPage);
            return View(viewModel);
        }
    }
}
