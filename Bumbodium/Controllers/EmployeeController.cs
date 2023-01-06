using Bumbodium.Data;
using Bumbodium.Data.DBModels;
using Bumbodium.Data.Migrations;
using Bumbodium.WebApp.Models;
using Bumbodium.WebApp.Models.ExcelExport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
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
            int employeesPerPage = 10;
            return View(new EmployeeListViewModel()
            {
                CurrentPage = 1,
                EmployeesPerPage = employeesPerPage,
                Employees = employees.Take(employeesPerPage),
                EmployeeCount = employees.Count()
            });
        }

        [HttpPost]
        public IActionResult Index(EmployeeListViewModel viewModel)
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

        // get create
        public IActionResult Create()
        {
            Employee employee = new();
            return View(new EmployeeCreateViewModel() { Employee = employee });
        }

        // post create
        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            else
            {
                IdentityUser user = new IdentityUser() { Email = viewModel.Employee.Email, UserName = viewModel.Employee.Email, PhoneNumber = viewModel.Employee.PhoneNumber };
                var result =  _userManager.CreateAsync(user, viewModel.Password).Result;
                if(!result.Succeeded)
                {
                    ModelState.AddModelError("UserCreateError", "Het aanmaken van de gebruiker ging fout, probeer het opnieuw");
                    return View(viewModel);
                }
                viewModel.Employee.EmployeeID = user.Id;
                _userManager.AddToRoleAsync(user, viewModel.Employee.Type.ToString());
                _employeeRepo.InsertEmployee(viewModel.Employee);
                _employeeRepo.AddEmployeeToDepartments(viewModel.Employee.EmployeeID, viewModel.Departments);
                return RedirectToAction("Index");
            }
        }

        // get edit
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employee employee = _employeeRepo.GetEmployee(id);
            return View(new EmployeeViewModel() { Employee = employee });
        }

        // post edit
        [HttpPost]
        public IActionResult Edit(EmployeeViewModel viewModel)
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

        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employee employee = _employeeRepo.GetEmployee(id);
            return View(employee);
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
