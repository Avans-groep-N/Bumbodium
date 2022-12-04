using Bumbodium.Data.DBModels;

namespace Bumbodium.WebApp.Models
{
    public class ForecastWeekViewModel
    {
        public Forecast[] DaysOfTheWeek { get; set; } = new Forecast[7];
    }
}
