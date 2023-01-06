using Bumbodium.Data;
using Bumbodium.Data.DBModels;
using Bumbodium.WebApp.Models;
using Bumbodium.WebApp.Models.ExcelExport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Bumbodium.WebApp.Controllers
{
    [Authorize(Roles = "Manager")]
    public class EmployeeController : Controller
    {
        private readonly EmployeeRepo _employeeRepo;
        private readonly UserManager<IdentityUser> _userManager;

        public EmployeeController(EmployeeRepo employeeRepo, UserManager<IdentityUser> userManager)
        {
            _employeeRepo = employeeRepo;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            IQueryable<Employee> employees = _employeeRepo.GetEmployees()
                .Include(e => e.PartOFDepartment)
                .ThenInclude(pod => pod.Department)
                .Where(e => e.DateOutService.HasValue == false)
                .OrderBy(e => e.FirstName);
            int employeesPerPage = 3;
            return View(new EmployeeIndexViewModel()
            {
                CurrentPage = 1,
                EmployeesPerPage = employeesPerPage,
                Employees = employees.Take(employeesPerPage),
                EmployeeCount = employees.Count()
            });
        }

        [HttpPost]
        public IActionResult Index(EmployeeIndexViewModel viewModel)
        {
            IQueryable<Employee> employees = _employeeRepo.GetEmployees()
                .Include(e => e.PartOFDepartment).ThenInclude(pod => pod.Department)
                .Where(e => e.DateOutService.HasValue == viewModel.ShowInactive);
            if (!string.IsNullOrEmpty(viewModel.NameFilter))
            {
                employees = employees.Where(e => (e.FirstName + " " + e.MiddleName + " " + e.LastName).ToLower().Contains(viewModel.NameFilter.ToLower()));
            }
            if (viewModel.DepartmentFilter > 0)
            {
                employees = employees.Where(e => e.PartOFDepartment.Any(pod => pod.DepartmentId == viewModel.DepartmentFilter));
            }
            viewModel.EmployeeCount = employees.Count();
            viewModel.Employees = employees.OrderBy(e => e.FirstName)
                .Skip(viewModel.EmployeesPerPage * (viewModel.CurrentPage - 1))
                .Take(viewModel.EmployeesPerPage);
            return View(viewModel);
        }

        // get edit
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employee employee = _employeeRepo.GetEmployee(id);
            return View(new EmployeeEditViewModel() { Employee = employee });
        }

        // post edit
        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Employee.PartOFDepartment = _employeeRepo.GetEmployee(viewModel.Employee.EmployeeID).PartOFDepartment;
                return View(viewModel);
            }
            else
            {
                IdentityUser user = _userManager.FindByIdAsync(viewModel.Employee.EmployeeID).Result;
                if (viewModel.Employee.Email != user.Email)
                {
                    string token = _userManager.GenerateChangeEmailTokenAsync(user, viewModel.Employee.Email).Result;
                    var result = _userManager.ChangeEmailAsync(user, viewModel.Employee.Email, token).Result;
                    if(!result.Succeeded)
                    {
                        ModelState.AddModelError("EmailInvalid", "De opgegeven Email was misvormd");
                        viewModel.Employee.PartOFDepartment = _employeeRepo.GetEmployee(viewModel.Employee.EmployeeID).PartOFDepartment;
                        return View(viewModel);
                    }
                }
                if (!string.IsNullOrEmpty(viewModel.Password))
                {
                    string token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
                    var result = _userManager.ResetPasswordAsync(user, token, viewModel.Password).Result;
                    if(!result.Succeeded)
                    {
                        ModelState.AddModelError("PasswordInvalid", "Wachtwoord moet minimaal een hoofdletter en een nummer hebben en uit meer dan 5 characters bestaan");
                        viewModel.Employee.PartOFDepartment = _employeeRepo.GetEmployee(viewModel.Employee.EmployeeID).PartOFDepartment;
                        return View(viewModel);
                    }
                }
                if (viewModel.Employee.Type == TypeStaff.Manager)
                {
                    ReplaceRoleWith("Employee", "Manager", user);
                }
                if (viewModel.Employee.Type == TypeStaff.Employee)
                {
                    ReplaceRoleWith("Manager", "Employee", user);
                }

                _employeeRepo.ReplaceDepartmentsOfEmployee(viewModel.Employee.EmployeeID, viewModel.Departments);

                _employeeRepo.UpdateEmployee(viewModel.Employee);
                _employeeRepo.UpdateUser(user);

                return RedirectToAction("Index");
            }
        }

        private void ReplaceRoleWith(string oldRole, string newRole, IdentityUser user)
        {
            if (!_userManager.GetRolesAsync(user).Result.Contains(newRole))
            {
                _userManager.RemoveFromRoleAsync(user, oldRole);
                _userManager.AddToRoleAsync(user, newRole);
            }
        }
    }
}
