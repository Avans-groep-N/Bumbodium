using Bumbodium.Data.DBModels;

namespace Bumbodium.WebApp.Models
{
    public class ForecastWeekViewModel
    {
        public Forecast[] DaysOfTheWeek { get; set; } = new Forecast[] {new Forecast() {
                    Date = DateTime.Parse("07 - 11 - 2022 22:37:00"),
                    AmountExpectedColis = 980,
                    AmountExpectedCustomers = 900 },
                new Forecast() {
                    Date = DateTime.Parse("08 - 11 - 2022 22:37:00"),
                    AmountExpectedColis = 880,
                    AmountExpectedCustomers = 860 }};
    }

//TODO: add a constructor 
}
