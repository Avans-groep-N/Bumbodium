using Bumbodium.Data;
using Bumbodium.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bumbodium.WebApp.Controllers
{
    public class EmployeeController : Controller
    {

        BumbodiumRepo _repo = new BumbodiumRepo();

        // GET: EmployeeController
        public ActionResult Index()
        {
            return View(_repo.GetEmployees());
        }

        // GET: EmployeeController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                return View(_repo.GetEmployee(id));
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            try
            {
                if (ValidateAccount(employee))
                {
                    _repo.CreateEmployee(employee);
                    _repo.CreateAccount(employee);

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                return View(_repo.GetEmployee(id));

            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee employee)
        {
            try
            {

                if (ValidateAccount(employee))
                {
                    _repo.EditEmployee(employee);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View();
                }

            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                return View(_repo.GetEmployee(id));
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Employee employee)
        {
            try
            {
                _repo.DeleteEmployee(employee);
                return RedirectToAction(nameof(Index));
            }

            catch
            {
                return View();
            }

        }

        public bool ValidateAccount(Employee employee)
        {
            int now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            int dob = int.Parse(employee.Birthdate.ToString("yyyyMMdd"));
            int age = (now - dob) / 10000;

            if (age < 15)
            {
                return false;
            }

            int result = DateTime.Compare(employee.DateInService, DateTime.Now);

            if (result > 0)
            {
                return false;
            }

            return true;
        }
    }
}
