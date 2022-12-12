using Bumbodium.Data.DBModels;

namespace Bumbodium.WebApp.Models
{
    public class ForecastWeek
    {
        public int WeekNr { get; set; }

        public List<ForecastItems> Days { get; set; }

        public ForecastWeek()
        {
            Days = new List<ForecastItems>();
        }
    }
}
