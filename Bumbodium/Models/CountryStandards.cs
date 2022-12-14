namespace Bumbodium.WebApp.Models
{
    public class CountryStandards
    {
        public string Country { get; set; }

        public List<StandardItem> StandardItems { get; set; }

        public CountryStandards()
        {
            StandardItems = new List<StandardItem>();
        }
    }
}
