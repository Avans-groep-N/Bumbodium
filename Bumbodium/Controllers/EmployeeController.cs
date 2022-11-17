using Bumbodium.Data;
using Bumbodium.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bumbodium.WebApp.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: EmployeeController
        public ActionResult Index()
        {
            using (var ctx = new BumbodiumContext())
            {
                return View(ctx.Employee.ToList());
            }
        }

        // GET: EmployeeController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                using (var ctx = new BumbodiumContext())
                {
                    return View(ctx.Employee.Find(id));
                }
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
                if (ModelState.IsValid)
                {
                    using (var ctx = new BumbodiumContext())
                    {
                        ctx.Employee.Add(employee);
                        ctx.SaveChanges();
                    }
                    return RedirectToAction(nameof(Index));

                }
                return View(employee);
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
                using (var ctx = new BumbodiumContext())
                {
                    return View(ctx.Employee.Find(id));
                }
              
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
                if (ModelState.IsValid)
                {
                    using (var ctx = new BumbodiumContext())
                    {
                        ctx.Attach(employee);
                        ctx.Employee.Update(employee);
                        ctx.SaveChanges();
                    }
                    return RedirectToAction(nameof(Index));

                }
                return View(employee);
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
                using var ctx = new BumbodiumContext();
                return View(ctx.Employee.Find(id));
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

                using (var ctx = new BumbodiumContext())
                {
                    ctx.Employee.Attach(employee);
                    ctx.Employee.Remove(employee);
                    ctx.SaveChanges();
                }
                return RedirectToAction(nameof(Index)); return View(employee);
            }
            catch
            {
                return View();
            }
        }
    }
}
