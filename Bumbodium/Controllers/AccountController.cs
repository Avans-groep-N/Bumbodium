using Bumbodium.Data;
using Bumbodium.Data.DBModels;
using Bumbodium.Data.Interfaces;
using Bumbodium.Data.Repositories;
using Bumbodium.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bumbodium.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly EmployeeRepo _EmployeeDb;
        private readonly DepartmentRepo _departmentRepo;
        private readonly BumbodiumContext _ctx;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            Microsoft.Extensions.Configuration.IConfiguration configuration,
            BumbodiumContext ctx)
        {
            _ctx = ctx;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _EmployeeDb = new EmployeeRepo(_ctx);
            _departmentRepo = new DepartmentRepo(_ctx);
        }

        public IActionResult Register()
        {
            InputModel input = new InputModel();
            PopulateInput(input);
            return View(input);
        }

        //TODO replace this with a proper Employee Create
        [HttpPost]
        public async Task<IActionResult> Register(InputModel input)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = input.Email,
                    Email = input.Email,
                    PhoneNumber = input.PhoneNumber
                };
                var result = await _userManager.CreateAsync(user, input.Password);
                if (result.Succeeded)
                {
                    //Employee
                    _EmployeeDb.InsertEmployee(new Employee()
                    {
                        EmployeeID = user.Id,
                        FirstName = input.FirstName,
                        MiddleName = input.MiddleName,
                        LastName = input.LastName,
                        Birthdate = input.Birthday,
                        PhoneNumber = input.PhoneNumber,
                        Email = user.Email,
                        DateInService = input.DateInService,    
                        Type = input.TypeStaff
                    }) ;

                    //Departments
                    foreach (var department in input.ActiveDepartmentIds)
                    {
                        _departmentRepo.AddEmployeeToDepartment(user.Id, department);
                    }
                    await _userManager.AddToRoleAsync(user, input.TypeStaff.ToString());
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    if(input.TypeStaff == TypeStaff.Employee)
                        return RedirectToAction("Index", "Availability");
                    else
                        return RedirectToAction("Index", "WeekSchedule");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            PopulateInput(input);
            return View(input);
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Manager"))
                {
                    return RedirectToAction("Index", "WeekSchedule");
                }
                if (User.IsInRole("Employee"))
                {
                    return RedirectToAction("Index", "Availability");
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginModel input)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(input.Email, input.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    IdentityUser currentUser = _EmployeeDb.GetUser(input.Email);
                    Employee employee = _EmployeeDb.GetEmployee(currentUser.Id);
                    if (employee.Type == Data.DBModels.TypeStaff.Manager)
                    {
                        return RedirectToAction("Index", "WeekSchedule");
                    }
                    else
                    {
                        //TODO Change to employee schedule once its made.
                        return RedirectToAction("Index", "Availability");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View();
                }
            }
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        private void PopulateInput(InputModel input)
        {
            input.DepartmentList = _departmentRepo.GetAllDepartments();
        }

        //actions for the debug buttons
        public async Task<IActionResult> DebugEmployeeLogin()
        {
            string email = "Henk@henk.nl";
            string password = "Henk";
            return await Index(new InputModel { Email = email, Password = password });
        }
        public async Task<IActionResult> DebugManagerLogin()
        {
            string email = "Johnny@vos.nl";
            string password = "Johnny";
            return await Index(new InputModel { Email = email, Password = password });
        }
        public async Task<IActionResult> DebugEmployee15Login()
        {
            string email = "Bliksem@martijnshamster.nl";
            string password = "Bliksem";
            return await Index(new InputModel { Email = email, Password = password });
        }
        public async Task<IActionResult> DebugEmployee17Login()
        {
            string email = "Lobbus@kjell.nl";
            string password = "Lobbus";
            return await Index(new InputModel { Email = email, Password = password });
        }
    }
}