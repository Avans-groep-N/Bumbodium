using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bumbodium.Data
{
    public class AvailablityData : IAvailablityData
    {
        private readonly ISqlDataAccess _db;
        public AvailablityData(ISqlDataAccess db)
        {
            _db = db;
        }
        public Task<List<Availability>> GetAvailabilities()
        {
            string sql = "select * from dbo.Availability";

            return _db.LoadData<Availability, dynamic>(sql, new { });
        }

        public Task InsertAvailablity(Availability availability)
        {
            string sql = @"insert into dbo.Availability (AvailabilityId, EmployeeId, StartDateTime, EndDateTime, Type) 
                        values (@AvailabilityId, @EmployeeId, @StartDateTime, @EndDateTime, @Type);";

            return _db.SaveData(sql, availability);
        }
    }
}
