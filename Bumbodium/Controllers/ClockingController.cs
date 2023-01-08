using Bumbodium.Data;
using Bumbodium.WebApp.Models.ClockingView;
using Bumbodium.WebApp.Models.Utilities.ClockingValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace Bumbodium.WebApp.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ClockingController : Controller
    {

        private BLClocking _blclocking;
        private EmployeeRepo _employeeRepo;

        public ClockingController(BLClocking blclocking, EmployeeRepo employeeRepo)
        {
            _blclocking = blclocking;
            _employeeRepo = employeeRepo;
        }

        public IActionResult Index()
        {
            var employeeList = _blclocking.GetEmployees();
            ViewBag.EmployeeList = new SelectList(employeeList, "Id", "Name");
            string id = "";
            if (employeeList.Count > 0)
                id = employeeList[0].Id;
            var clockingViewModel = _blclocking.GetClockingViewModel(id, ISOWeek.GetWeekOfYear(DateTime.Now), DateTime.Now.Year);
            return View(clockingViewModel);
        }

        [HttpPost]
        public IActionResult SelectWeek()
        {
            var id = Request.Form["Id"];

            if (id.Equals(""))
                return RedirectToAction(nameof(Index));

            var week = Request.Form["weeknumber"].First().Split("-W");
            int[] yearAndWeek = { Int32.Parse(week[0]), Int32.Parse(week[1]) };


            var clockingViewModel = _blclocking.GetClockingViewModel(id, yearAndWeek[1], yearAndWeek[0]);
            AddEmployeeToViewBag(id);

            return View("../Clocking/Index", clockingViewModel);
        }

        private void AddEmployeeToViewBag(string id)
        {
            var employeeList = _blclocking.GetEmployees();
            var temp = employeeList.Find(e => e.Id == id);
            if (temp != null)
            {
                employeeList.Remove(temp);
                employeeList.Insert(0, temp);
            }
            ViewBag.EmployeeList = new SelectList(employeeList, "Id", "Name");
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public ActionResult SaveNewTimes()
        {
            var employeeId = Request.Form["employeeId"].Single();
            var day = Request.Form["day"];
            var startTime = Request.Form["timestart"];
            var alterdStartTime = Request.Form["alterdtimestart"];
            var endTime = Request.Form["timeend"];
            var alterdEndTime = Request.Form["alterdtimeend"];

            for (int i = 0; i < day.Count; i++)
            {
                var mCItem = new ManagerClockingItem();

                mCItem.ClockStartTime = DateTime.Parse(day[i] + " " + startTime[i]);
                mCItem.AlterdClockStartTime = DateTime.Parse(day[i] + " " + alterdStartTime[i]);
                mCItem.ClockEndTime = DateTime.Parse(day[i] + " " + endTime[i]);
                mCItem.AlterdClockEndTime = DateTime.Parse(day[i] + " " + alterdEndTime[i]);

                if (mCItem.ClockStartTime != mCItem.AlterdClockStartTime || mCItem.ClockEndTime != mCItem.AlterdClockEndTime)
                    _blclocking.Save(employeeId, mCItem);

            }
            DateTime date = DateTime.Parse(day[0]);
            while(date.DayOfWeek != DayOfWeek.Monday)
                date = date.AddDays(-1);

            int[] yearAndWeek = new int[] { date.Year, ISOWeek.GetWeekOfYear(date) };

            var clockingViewModel = _blclocking.GetClockingViewModel(employeeId, yearAndWeek[1], yearAndWeek[0]);
            AddEmployeeToViewBag(employeeId);


            return View("../Clocking/Index", clockingViewModel);
        }
    }
}
