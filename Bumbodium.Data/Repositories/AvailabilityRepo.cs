using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bumbodium.Data.DBModels;
using Bumbodium.Data.Interfaces;

namespace Bumbodium.Data
{
    public class AvailabilityRepo : IAvailablityRepo
    {
        private readonly ISqlDataAccess _db;
        public AvailabilityRepo(ISqlDataAccess db)
        {
            _db = db;
        }
        public Task<List<Availability>> GetAvailabilities()
        {
            string sql = "select * from dbo.Availability";

            return _db.LoadData<Availability, dynamic>(sql, new { });
        }

        public Task<List<Availability>> GetAvailabilitiesInRange(DateTime start, DateTime end)
        {
            string sql = "select * from dbo.Availability " +
                "where StartDateTime between '" + start.ToString("yyyy-MM-dd") + "' AND '" + end.ToString("yyyy-MM-dd") + "'";
            return _db.LoadData<Availability, dynamic>(sql, new { });
        }

        public Task InsertAvailability(Availability availability)
        {
            string sql = @"insert into dbo.Availability (EmployeeId, StartDateTime, EndDateTime, Type) 
                        values (@EmployeeId, @StartDateTime, @EndDateTime, @Type);";

            return _db.SaveData(sql, availability);
        }

        public Task DeleteAvailability(Availability availability)
        {
            string sql = @"delete from dbo.Availability where EmployeeId = @EmployeeId 
                        AND AvailabilityId = @AvailabilityId;";

            return _db.SaveData(sql, availability);
        }

        public Task UpdateAvailability(Availability availability)
        {
            string sql = @"update dbo.Availability 
                        set StartDateTime = @StartDateTime, EndDateTime = @EndDateTime, Type = @Type
                        where EmployeeId = @EmployeeId 
                        AND AvailabilityId = @AvailabilityId;;";

            return _db.SaveData(sql, availability);
        }
    }
}
