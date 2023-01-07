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

            var clockingViewModel = new ClockingViewModel() { WeekNumber = 48, YearNumber = 2023 };

            var clockingFirstDayViewModel = new ClockingDayViewModel() { Day = DateTime.Now};
            var clockingSecondDayViewModel = new ClockingDayViewModel() { Day = DateTime.Now };
            var clockingThirdDayViewModel = new ClockingDayViewModel() { Day = DateTime.Now };

            clockingFirstDayViewModel.ManagerClocking.Add(new ManagerClockingItem()
            {
                ClockStartTime = new DateTime(2022, 12, 1, 12, 54, 0),
                ClockEndTime = new DateTime(2022, 12, 1, 18, 12, 0),
                IsChanged = false,
                IsOnGoing = false,
                ScheduleStartTime = new DateTime(2022, 12, 1, 13, 00, 0),
                ScheduleEndTime = new DateTime(2022, 12, 1, 18, 00, 0)
            });

            clockingSecondDayViewModel.ManagerClocking.Add(new ManagerClockingItem()
            {
                ClockStartTime = new DateTime(2022, 12, 4, 12, 57, 0),
                ClockEndTime = new DateTime(2022, 12, 4, 19, 00, 0),
                IsChanged = true,
                IsOnGoing = false,
                ScheduleStartTime = new DateTime(2022, 12, 4, 13, 00, 0),
                ScheduleEndTime = new DateTime(2022, 12, 4, 18, 00, 0)

            });

            clockingThirdDayViewModel.EmployeeClocking.Add(new MedewerkerClockingItem()
            {
                ClockStartTime = new DateTime(2022, 12, 5, 12, 51, 0),
                ClockEndTime = new DateTime(2022, 12, 5, 15, 04, 0),
                IsChanged = false,
                IsOnGoing = true,
                ScheduleStartTime = new DateTime(2022, 12, 5, 12, 00, 0),
                ScheduleEndTime = new DateTime(2022, 12, 5, 15, 00, 0)

            });

            clockingThirdDayViewModel.EmployeeClocking.Add(new MedewerkerClockingItem()
            {
                ClockStartTime = new DateTime(2022, 12, 5, 17, 53, 0),
                ClockEndTime = new DateTime(2022, 12, 5, 21, 13, 0),
                IsChanged = false,
                IsOnGoing = false,
                ScheduleStartTime = new DateTime(2022, 12, 5, 18, 00, 0),
                ScheduleEndTime = new DateTime(2022, 12, 5, 21, 00, 0)
            });

            clockingViewModel.ClockingDays.Add(clockingFirstDayViewModel);
            clockingViewModel.ClockingDays.Add(clockingSecondDayViewModel);
            clockingViewModel.ClockingDays.Add(clockingThirdDayViewModel);

            //var clockingViewModel = _blclocking.GetClockingViewModel("19f7d479-542a-408b-9016-0561e3e70f65", 53, 2020);


            return View(clockingViewModel);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public ActionResult SaveNewTimes(ClockingViewModel clockingVM)
        {

            _ = clockingVM;

            //_blclocking.Save();



            return Redirect(nameof(Index));
        }

    }
}
