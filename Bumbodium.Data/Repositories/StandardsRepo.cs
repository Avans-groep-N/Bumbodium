using Bumbodium.Data.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
