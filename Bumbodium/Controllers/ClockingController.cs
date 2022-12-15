using Bumbodium.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bumbodium.WebApp.Controllers
{
    public class ClockingController : Controller
    {
        public IActionResult Index()
        {
            var clockingViewModel = new ClockingViewModel() { WeekNumber = 48 };
            
            var clockingFirstDayViewModel = new ClockingDayViewModel();
            var clockingSecondDayViewModel = new ClockingDayViewModel();
            var clockingThirdDayViewModel = new ClockingDayViewModel();
           
            clockingFirstDayViewModel.ClockingItems.Add(new ClockingItemViewModel()
            {
                ClockStartTime = new DateTime(2022, 12, 1, 12, 54, 0),
                ClockEndTime = new DateTime(2022, 12, 1, 18, 12, 0),
                IsChanged = false,
                IsOnGoing = false,
                ScheduleStartTime = new DateTime(2022, 12, 1, 13, 00, 0),
                ScheduleEndTime = new DateTime(2022, 12, 1, 18, 00, 0)
            });

            clockingSecondDayViewModel.ClockingItems.Add(new ClockingItemViewModel()
            {
                ClockStartTime = new DateTime(2022, 12, 4, 12, 57, 0),
                ClockEndTime = new DateTime(2022, 12, 4, 19, 00, 0),
                IsChanged = false,
                IsOnGoing = false,
                ScheduleStartTime = new DateTime(2022, 12, 4, 13, 00, 0),
                ScheduleEndTime = new DateTime(2022, 12, 4, 18, 00, 0)

            });

            clockingThirdDayViewModel.ClockingItems.Add(new ClockingItemViewModel()
            {
                ClockStartTime = new DateTime(2022, 12, 5, 12, 51, 0),
                ClockEndTime = new DateTime(2022, 12, 5, 15, 04, 0),
                IsChanged = false,
                IsOnGoing = false,
                ScheduleStartTime = new DateTime(2022, 12, 5, 12, 00, 0),
                ScheduleEndTime = new DateTime(2022, 12, 5, 15, 00, 0)

            });

            clockingThirdDayViewModel.ClockingItems.Add(new ClockingItemViewModel()
            {
                ClockStartTime = new DateTime(2022, 12, 5, 17, 53, 0),
                ClockEndTime =  new DateTime(2022, 12, 5, 21, 13, 0),
                IsChanged = false,
                IsOnGoing = false,
                ScheduleStartTime = new DateTime(2022, 12, 5, 18, 00, 0),
                ScheduleEndTime = new DateTime(2022, 12, 5, 21, 00, 0)
            });

            clockingViewModel.ClockingDays.Add(clockingFirstDayViewModel);
            clockingViewModel.ClockingDays.Add(clockingSecondDayViewModel);
            clockingViewModel.ClockingDays.Add(clockingThirdDayViewModel);

            return View(clockingViewModel);
    }
}
}
