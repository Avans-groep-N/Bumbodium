using Bumbodium.Data.DBModels;
using Microsoft.AspNetCore.Mvc;

namespace Bumbodium.WebApp.Controllers
{
    public class StandardController : Controller
    {
        public IActionResult Index()
        {
            var standards = new Standards[] {
                new Standards() {
                Country = "Netherlands",
                Description = "Coli uitladen per minuten",
                Value = 5 },

                new Standards() {
                Country = "Netherlands",
                Description = "Vakken vullen, minuten per coli",
                Value = 30 },

                new Standards() {
                Country = "Netherlands",
                Description = "1 Kasiere per uur per klanten",
                Value = 30 },

                new Standards() {
                Country = "Netherlands",
                Description = "1 medewerker per uur voor klanten",
                Value = 100},

                new Standards() {
                Country = "Netherlands",
                Description = "Spiegelen aantal seconde per meter",
                Value = 30},

            };
            return View(standards);
        }
    }
}
