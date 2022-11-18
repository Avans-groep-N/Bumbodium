﻿using Bumbodium.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bumbodium.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private Data.BumboRepo _repo = new Data.BumboRepo();

        public HomeController(ILogger<HomeController> logger)
        {
        }

        public IActionResult Index()
        {   
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new Account());
        }
        [HttpPost]
        public IActionResult Login(Account account)
        {
            Data.Account dbAccount = new Data.Account() { Username = account.Username, Password = account.Password };

            //If account matches account in db, return to homepage, otherwise do nothing
            if (_repo.ValidateAccount(dbAccount))
                return RedirectToAction("Index");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}