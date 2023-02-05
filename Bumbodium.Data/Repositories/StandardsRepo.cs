using Bumbodium.Data.DBModels;

namespace Bumbodium.Data.Repositories
{
    public class StandardsRepo
    {
        private BumbodiumContext _ctx;

        public StandardsRepo(BumbodiumContext ctx)
        {
            _ctx = ctx;
        }

        public List<Standards> GetAll(Country country) => _ctx.Standards.Where(s => s.Country == country).ToList();

        public void UpdateStandards(List<Standards> standardsDB)
        {
            _ctx.Standards.UpdateRange(standardsDB);
            _ctx.SaveChanges();
        }
    }
}
