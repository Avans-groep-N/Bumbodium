using Bumbodium.Data.DBModels;
using Bumbodium.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bumbodium.Data.Repositories
{
    public class ShiftRepo
    {
        private readonly ISqlDataAccess _db;
        public ShiftRepo(ISqlDataAccess db)
        {
            _db = db;
        }
        
        public Task<List<Shift>> GetShifts()
        {
            return null;
        }
        //add a filter based on department in the function
        public Task<List<Shift>> GetFilteredShifts()
        {
            return null;
        }
    }
}
