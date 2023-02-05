using Bumbodium.Data;
using Bumbodium.Data.DBModels;
using Bumbodium.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
            int employeesPerPage = 10;
            int employeeCount = _employeeRepo.GetEmployeesFiltered(null, null, false).Count();
            List<Employee> employees = _employeeRepo.GetEmployeesList(null, null, 0, employeesPerPage, false);
            return View(new EmployeeListViewModel()
            {
                CurrentPage = 1,
                EmployeesPerPage = employeesPerPage,
                Employees = employees,
                EmployeeCount = employeeCount
            });
        }

        [HttpPost]
        public IActionResult Index(EmployeeListViewModel viewModel)
        {
            viewModel.EmployeeCount = _employeeRepo.GetEmployeesFiltered(viewModel.NameFilter, viewModel.DepartmentFilter, viewModel.ShowInactive).Count();
            viewModel.Employees = _employeeRepo.GetEmployeesList(viewModel.NameFilter, viewModel.DepartmentFilter, viewModel.EmployeesPerPage * (viewModel.CurrentPage - 1), viewModel.EmployeesPerPage, viewModel.ShowInactive);
            return View(viewModel);
        }

        public IActionResult Create()
        {
            Employee employee = new();
            return View(new EmployeeCreateViewModel() { Employee = employee });
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            else
            {
                IdentityUser user = new IdentityUser() { Email = viewModel.Employee.Email, UserName = viewModel.Employee.Email, PhoneNumber = viewModel.Employee.PhoneNumber };
                var result = await _userManager.CreateAsync(user, viewModel.Password);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("UserCreateError", "Het aanmaken van de gebruiker ging fout, probeer het opnieuw");
                    return View(viewModel);
                }
                viewModel.Employee.EmployeeID = user.Id;
                await _userManager.AddToRoleAsync(user, viewModel.Employee.Type.ToString());
                _employeeRepo.InsertEmployee(viewModel.Employee);
                _employeeRepo.AddEmployeeToDepartments(viewModel.Employee.EmployeeID, viewModel.Departments);
                return RedirectToAction("Index");
            }
        }

        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employee employee = _employeeRepo.GetEmployee(id);
            return View(new EmployeeViewModel() { Employee = employee });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeViewModel viewModel)
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
                    string token = await _userManager.GenerateChangeEmailTokenAsync(user, viewModel.Employee.Email);
                    var result = await _userManager.ChangeEmailAsync(user, viewModel.Employee.Email, token);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("EmailInvalid", "De opgegeven Email was misvormd");
                        viewModel.Employee.PartOFDepartment = _employeeRepo.GetEmployee(viewModel.Employee.EmployeeID).PartOFDepartment;
                        return View(viewModel);
                    }
                }
                if (!string.IsNullOrEmpty(viewModel.Password))
                {
                    string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, viewModel.Password);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("PasswordInvalid", "Wachtwoord moet minimaal een hoofdletter en een nummer hebben en uit meer dan 5 characters bestaan");
                        viewModel.Employee.PartOFDepartment = _employeeRepo.GetEmployee(viewModel.Employee.EmployeeID).PartOFDepartment;
                        return View(viewModel);
                    }
                }
                if (viewModel.Employee.Type == TypeStaff.Manager)
                {
                    await ReplaceRoleWith("Employee", "Manager", user);
                }
                if (viewModel.Employee.Type == TypeStaff.Employee)
                {
                    await ReplaceRoleWith("Manager", "Employee", user);
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


        private async Task ReplaceRoleWith(string oldRole, string newRole, IdentityUser user)
        {
            if (!(await _userManager.GetRolesAsync(user)).Contains(newRole))
            {
                await _userManager.RemoveFromRoleAsync(user, oldRole);
                await _userManager.AddToRoleAsync(user, newRole);
            }
        }
    }
}
