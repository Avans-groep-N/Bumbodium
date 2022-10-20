using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bumbodium.Data;

namespace Bumbodium.Controllers
{
    public class WeekScheduleController : Controller
    {
        // GET: WeekRoosterController
        public ActionResult Index()
        {
            List<Shift> shifts = new List<Shift>();
            return View(shifts);
        }

        // GET: WeekRoosterController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WeekRoosterController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WeekRoosterController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WeekRoosterController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WeekRoosterController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WeekRoosterController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WeekRoosterController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
