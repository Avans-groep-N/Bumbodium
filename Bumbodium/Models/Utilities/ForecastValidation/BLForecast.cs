using Bumbodium.Data.DBModels;
using Bumbodium.Data.Repositories;
using System.Globalization;

namespace Bumbodium.WebApp.Models.Utilities.ForecastValidation
{
    public class BLForecast
    {
        private ForecastRepo _forcastRepo;

        public BLForecast(ForecastRepo forecastRepo)
        {
            _forcastRepo = forecastRepo;
        }





        public ForecastWeek GetForecastWeek(int year, int weekNr)
        {
            //TODO: List of departments with the right branchid
            List<int> departmentIds = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var startDate = FirstDateOfWeekISO8601(year, weekNr);
            var endDate = startDate.AddDays(7);

            var forecasts = _forcastRepo.GetAllInRange(startDate, endDate, departmentIds);

            ForecastWeek forecastWeek = new ForecastWeek() { WeekNr = weekNr };
            FillForecastWeek(forecastWeek, forecasts, startDate);

            return forecastWeek;
        }

        private void FillForecastWeek(ForecastWeek fw, List<Forecast> forecasts, DateTime startDate)
        {
            for (int dayOff = 0; dayOff < 7; dayOff++)
            {
                List<Forecast> forPerDay = forecasts.Where(f => f.Date.DayOfWeek == startDate.AddDays(dayOff).DayOfWeek).ToList();
                ForecastItems fi;
                if (forPerDay.Count != 0)
                    fi = FillForecastDay(startDate.AddDays(dayOff), forPerDay);
                else
                    fi = FillEmptyForecastDay(startDate.AddDays(dayOff));
                fw.Days.Add(fi);
            }
        }

        private ForecastItems FillEmptyForecastDay(DateTime dayOfWeek)
        {
            return new ForecastItems()
            {
                Date = dayOfWeek,
                AmountColis = 0,
                AmountCustommers = 0,
                AmountEmployees = 0,
                AmountHours = 0,
                AllowedToChange = dayOfWeek >= DateTime.Today
            };
        }

        private ForecastItems FillForecastDay(DateTime dayOfWeek, IEnumerable<Forecast> forPerDay)
        {
            return new ForecastItems()
            {
                Date = dayOfWeek,
                AmountColis = forPerDay.Sum(f => f.AmountExpectedColis),
                AmountCustommers = forPerDay.Sum(f => f.AmountExpectedCustomers),
                AmountEmployees = forPerDay.Sum(f => f.AmountExpectedEmployees),
                AmountHours = forPerDay.Sum(f => f.AmountExpectedHours),
                AllowedToChange = dayOfWeek >= DateTime.Today
            };
        }

        
        
        
        public DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            // Use first Thursday in January to get first week of the year as
            // it will never be in Week 52/53
            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            // As we're adding days to a date in Week 1,
            // we need to subtract 1 in order to get the right date for week #1
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }

            // Using the first Thursday as starting week ensures that we are starting in the right year
            // then we add number of weeks multiplied with days
            var result = firstThursday.AddDays(weekNum * 7);

            // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
            return result.AddDays(-3);
        }
    }
}
