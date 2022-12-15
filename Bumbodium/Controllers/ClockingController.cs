using Bumbodium.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bumbodium.WebApp.Controllers
{
    public class ClockingController : Controller
    {
        public IActionResult Index()
        {
            var clockingViewModel = new ClockingViewModel() { WeekNumber = 48};
            clockingViewModel.ClockingItems.Add(new ClockingItemViewModel()
            {
                ClockStartTime = new DateTime(2022, 12, 1, 12, 54, 0),
                ClockEndTime = new DateTime(2022, 12, 1, 18, 12, 0),
                IsChanged = false,
                IsOnGoing = false,
                ScheduleStartTime = new DateTime(2022, 12, 1, 13, 00, 0),
                ScheduleEndTime = new DateTime(2022, 12, 1, 18, 00, 0)

            });
            clockingViewModel.ClockingItems.Add(new ClockingItemViewModel()
            {
                ClockStartTime = new DateTime(2022, 12, 4, 12, 57, 0),
                ClockEndTime = new DateTime(2022, 12, 4, 19, 00, 0),
                IsChanged = false,
                IsOnGoing = false,
                ScheduleStartTime = new DateTime(2022, 12, 4, 13, 00, 0),
                ScheduleEndTime = new DateTime(2022, 12, 4, 18, 00, 0)

            });
            clockingViewModel.ClockingItems.Add(new ClockingItemViewModel()
            {
                ClockStartTime = new DateTime(2022, 12, 5, 12, 51, 0),
                ClockEndTime = new DateTime(2022, 12, 5, 18, 04, 0),
                IsChanged = false,
                IsOnGoing = false,
                ScheduleStartTime = new DateTime(2022, 12, 5, 13, 00, 0),
                ScheduleEndTime = new DateTime(2022, 12, 5, 18, 00, 0)

            });
            return View(clockingViewModel);
        }
    }
}
