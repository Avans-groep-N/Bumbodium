using Bumbodium.Data.DBModels;
using Bumbodium.Data.Repositories;

namespace Bumbodium.WebApp.Models.Utilities.StandardsValidation
{
    public class BLStandards
    {
        private StandardsRepo _standardsRepo;

        public BLStandards(StandardsRepo standardsRepo)
        {
            _standardsRepo = standardsRepo;
        }

        public CountryStandards GetCountryStandards(Country country)
        {
            var standards = _standardsRepo.GetAllInRange(country);

            var coSt = new CountryStandards() { Country = country.ToString()};

            FillCountryStandards(coSt, standards);
            return coSt;
        }

        private void FillCountryStandards(CountryStandards coSt, List<Standards> standards)
        {
            if (standards.Count == 0)
            {
                coSt.StandardItems.Add(new StandardItem() { Activities = "InCorrect Data!!", Description = "InCorrect Data!!", Value = -1});
                return;
            }

            foreach (var standard in standards)
            {
                var item = new StandardItem()
                {
                    Activities = standard.Subject,
                    Value = standard.Value,
                    Description = standard.Description
                };

                coSt.StandardItems.Add(item);
            }
        }
    }
}
