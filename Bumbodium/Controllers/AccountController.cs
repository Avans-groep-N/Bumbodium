using Bumbodium.Data;
using Bumbodium.Data.DBModels;
using Bumbodium.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
                    //TODO Change to employee schedule once its made.
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
    }
}