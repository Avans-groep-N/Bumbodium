using Bumbodium.Data;
using Bumbodium.Data.DBModels;
using Bumbodium.Data.Interfaces;
using Bumbodium.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly EmployeeRepo _db;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _db = new EmployeeRepo(new SqlDataAccess(config: _configuration));
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(InputModel input)
        {
            if (ModelState.IsValid)
            {

                var user = new IdentityUser { 
                    UserName = input.FirstName + input.MiddleName + input.LastName, 
                    Email = input.Email,
                    PhoneNumber = input.PhoneNumber
                };
                var result = await _userManager.CreateAsync(user, input.Password);
                if (result.Succeeded)
                {
                    await _db.InsertEmployee(new Employee() { 
                        EmployeeID = user.Id, 
                        FirstName = input.FirstName,
                        MiddleName = input.MiddleName,
                        LastName = input.LastName,
                        Birthdate = input.Birthday,
                        PhoneNumber = input.PhoneNumber,
                        Email = user.Email});    
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel input)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(input.Email, input.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var currentUserTask = await _db.GetUser(input.Email);
                    IdentityUser currentUser = currentUserTask.FirstOrDefault();
                    var employeeTask = await _db.GetEmployee(currentUser);
                    Employee employee = employeeTask.FirstOrDefault();
                    if (employee.Type == Data.DBModels.TypeStaff.Manager)
                        return RedirectToAction("Index", "Home");
                    else
                        return RedirectToAction("EmployeeIndex", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View();
                }
            }
            return View();
        }
    }
}