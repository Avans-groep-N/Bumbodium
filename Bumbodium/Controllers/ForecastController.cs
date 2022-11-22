using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bumbodium.WebApp.Controllers
{
    public class ForecastController : Controller
    {
        // GET: ForecastController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ForecastController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ForecastController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ForecastController/Create
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

        // GET: ForecastController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ForecastController/Edit/5
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

        // GET: ForecastController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ForecastController/Delete/5
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
