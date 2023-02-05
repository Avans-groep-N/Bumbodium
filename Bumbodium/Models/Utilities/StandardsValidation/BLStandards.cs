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

        public StandardsViewModel GetStandardsViewModel(Country country)
        {
            List<Standards> standardsDB = _standardsRepo.GetAll(country);

            var standardsVW = new StandardsViewModel();
            standardsVW.Country = country;

            foreach (Standards standardDB in standardsDB)
            {
                standardsVW.AddToStandards(new Standard()
                {
                    Description = standardDB.Description,
                    Subject = standardDB.Subject,
                    Value = standardDB.Value
                });
            }

            return standardsVW;
        }

        public void ChangeStandards(StandardsViewModel standardsVM)
        {
            List<Standards> standardsDB = _standardsRepo.GetAll(standardsVM.Country);

            foreach (var standard in standardsVM.Standards)
            {
                var changingS = standardsDB.FirstOrDefault(s => s.Subject.Equals(standard.Subject));
                if (changingS == null || standard.Value == changingS.Value)
                    continue;

                changingS.Value = standard.Value;
            }

            _standardsRepo.UpdateStandards(standardsDB);
        }
    }
}
