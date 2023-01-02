using Bumbodium.Data;
using Bumbodium.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            return View(new EmployeeIndexViewModel()
            {
                Employees = _employeeRepo.GetEmployees()
            });
        }
    }
}
