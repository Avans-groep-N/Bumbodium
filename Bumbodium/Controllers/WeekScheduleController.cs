using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bumbodium.Data.DBModels;

namespace Bumbodium.WebApp.Controllers
{
    public class WeekScheduleController : Controller
    {
        // GET: WeekRoosterController
        public ActionResult Index()
        {
            var shifts = new Shift[] { new Shift() {
                DepartmentId = DepartmentType.Vegetables_Fruit,
                ShiftEndDateTime = DateTime.Now,
                ShiftStartDateTime = DateTime.Today,
                ShiftId = 1} };
            return View(shifts);
        }
    }
}
