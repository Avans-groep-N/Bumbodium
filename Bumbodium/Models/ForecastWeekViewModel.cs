using Bumbodium.Data;

namespace Bumbodium.WebApp.Models
{
    public class ForecastWeekViewModel
    {
        //public ForecastDayViewModel[] DaysOfTheWeek { get; set; } = new ForecastDayViewModel[2];
        public Forecast[] DaysOfTheWeek { get; set; } = new Forecast[] {new Forecast() {
                    Date = DateTime.Parse("07 - 11 - 2022 22:37:00"),
                    AmountExpectedColis = 980,
                    AmountExpectedCustomers = 900 },
                new Forecast() {
                    Date = DateTime.Parse("08 - 11 - 2022 22:37:00"),
                    AmountExpectedColis = 880,
                    AmountExpectedCustomers = 860 }};
    }

/*    public class ForecastDayViewModel
    {
        public DateOnly Date { get; set; }

        public int AmountExpectedCustomers { get; set; }

        public int AmountExpectedColis { get; set; }
    }*/
}
