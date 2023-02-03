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
        private readonly EmployeeRepo _employeeRepo;

        public AccountController(
            SignInManager<IdentityUser> signInManager,
            EmployeeRepo employeeRepo)
        {
            _employeeRepo = employeeRepo;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Manager"))
                {
                    return RedirectToAction("Index", "ManagerSchedule");
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
                    IdentityUser currentUser = _employeeRepo.GetUser(input.Email);
                    Employee employee = _employeeRepo.GetEmployee(currentUser.Id);
                    if (employee.Type == Data.DBModels.TypeStaff.Manager)
                    {
                        return RedirectToAction("Index", "ManagerSchedule");
                    }
                    else
                    {
                        //TODO Change to employee schedule once its made.
                        return RedirectToAction("Index", "Availability");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Gebruikersnaam en/of wachtwoord is verkeerd");
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

        //actions for the debug buttons
        public async Task<IActionResult> DebugEmployeeLogin()
        {
            string email = "Henk@henk.nl";
            string password = "Henk";
            return await Index(new LoginModel { Email = email, Password = password });
        }
        public async Task<IActionResult> DebugManagerLogin()
        {
            string email = "Johnny@vos.nl";
            string password = "Johnny";
            return await Index(new LoginModel { Email = email, Password = password });
        }
        public async Task<IActionResult> DebugEmployee15Login()
        {
            string email = "Bliksem@martijnshamster.nl";
            string password = "Bliksem";
            return await Index(new LoginModel { Email = email, Password = password });
        }
        public async Task<IActionResult> DebugEmployee17Login()
        {
            string email = "Lobbus@kjell.nl";
            string password = "Lobbus";
            return await Index(new LoginModel { Email = email, Password = password });
        }
    }
}