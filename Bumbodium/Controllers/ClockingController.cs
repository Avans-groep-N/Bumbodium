using Bumbodium.WebApp.Models.ClockingView;
using Bumbodium.WebApp.Models.Utilities.ClockingValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bumbodium.WebApp.Controllers
{
    public class ClockingController : Controller
    {
        private BLClocking _blclocking;

        public ClockingController(BLClocking blclocking)
        {
            _blclocking = blclocking;
        }

        [Authorize(Roles = "Manager")]
        public IActionResult Index()
        {

            /*var clockingViewModel = new ClockingViewModel() { EmployeeName = "sukkel", EmployeeId = "19f7d479-542a-408b-9016-0561e3e70f65", WeekNumber = 48, YearNumber = 2022 };

            var clockingFirstDayViewModel = new ClockingDayViewModel() { Day = new DateTime(2022, 12, 1, 12, 54, 0) };
            var clockingSecondDayViewModel = new ClockingDayViewModel() { Day = new DateTime(2022, 12, 4, 12, 57, 0) };
            var clockingThirdDayViewModel = new ClockingDayViewModel() { Day = new DateTime(2022, 12, 5, 12, 51, 0) };

            clockingFirstDayViewModel.ManagerClocking.Add(new ManagerClockingItem()
            {
                ClockStartTime = new DateTime(2022, 12, 1, 12, 54, 0),
                ClockEndTime = new DateTime(2022, 12, 1, 18, 12, 0),
                ScheduleStartTime = new DateTime(2022, 12, 1, 13, 00, 0),
                ScheduleEndTime = new DateTime(2022, 12, 1, 18, 00, 0)
            });

            clockingSecondDayViewModel.ManagerClocking.Add(new ManagerClockingItem()
            {
                ClockStartTime = new DateTime(2022, 12, 4, 12, 57, 0),
                ClockEndTime = new DateTime(2022, 12, 4, 19, 00, 0),
                ScheduleStartTime = new DateTime(2022, 12, 4, 13, 00, 0),
                ScheduleEndTime = new DateTime(2022, 12, 4, 18, 00, 0)

            });

            clockingThirdDayViewModel.ManagerClocking.Add(new ManagerClockingItem()
            {
                ClockStartTime = new DateTime(2022, 12, 5, 12, 51, 0),
                ClockEndTime = new DateTime(2022, 12, 5, 15, 04, 0),
                ScheduleStartTime = new DateTime(2022, 12, 5, 12, 00, 0),
                ScheduleEndTime = new DateTime(2022, 12, 5, 15, 00, 0)

            });

            clockingThirdDayViewModel.ManagerClocking.Add(new ManagerClockingItem()
            {
                ClockStartTime = new DateTime(2022, 12, 5, 17, 53, 0),
                ClockEndTime = new DateTime(2022, 12, 5, 21, 13, 0),
                ScheduleStartTime = new DateTime(2022, 12, 5, 18, 00, 0),
                ScheduleEndTime = new DateTime(2022, 12, 5, 21, 00, 0)
            });

            clockingViewModel.ClockingDays.Add(clockingFirstDayViewModel);
            clockingViewModel.ClockingDays.Add(clockingSecondDayViewModel);
            clockingViewModel.ClockingDays.Add(clockingThirdDayViewModel);*/

            var clockingViewModel = _blclocking.GetClockingViewModel("19f7d479-542a-408b-9016-0561e3e70f65", 53, 2020);


            return View(clockingViewModel);
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
            return Redirect(nameof(Index));
        }
    }
}
