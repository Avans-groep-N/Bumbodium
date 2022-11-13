using Bumbodium.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

namespace Bumbodium.WebApp.Controllers
{
    public class AccountController : Controller
    {
        // GET: AccountController
        public ActionResult Index()
        {
            using (var ctx = new BumbodiumContext())
            {
                return View(ctx.Accounts.ToList());
            }
        }

        // GET: AccountController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                using (var ctx = new BumbodiumContext())
                {
                    return View(ctx.Accounts.Find(id));
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Account account)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var ctx = new BumbodiumContext())
                    {
                        ctx.Accounts.Add(account);
                        ctx.SaveChanges();
                    }
                    return RedirectToAction(nameof(Index));

                }
                return View(account);
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                using (var ctx = new BumbodiumContext())
                {
                    ctx.Accounts.Find(id);
                }
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: AccountController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Account account)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var ctx = new BumbodiumContext())
                    {
                        ctx.Attach(account);
                        ctx.Accounts.Update(account);
                        ctx.SaveChanges();
                    }
                    return RedirectToAction(nameof(Index));

                }
                return View(account);
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                using var ctx = new BumbodiumContext();
                return View(ctx.Accounts.Find(id));
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: AccountController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Account account)
        {
            try
            {

                using (var ctx = new BumbodiumContext())
                {
                    ctx.Accounts.Attach(account);
                    ctx.Accounts.Remove(account);
                    ctx.SaveChanges();
                }
                return RedirectToAction(nameof(Index)); return View(account);
            }
            catch
            {
                return View();
            }
        }
    }
    }

