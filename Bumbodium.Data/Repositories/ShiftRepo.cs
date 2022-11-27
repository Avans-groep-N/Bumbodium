using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bumbodium.Data.DBModels;
using Bumbodium.Data.Interfaces;

namespace Bumbodium.Data
{
    public class ShiftRepo : IShiftRepo
    {
        private readonly ISqlDataAccess _db;
        public ShiftRepo(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<List<Shift>> GetShiftsInRange(DateTime start, DateTime end)
        {
            string sql = "select * from dbo.Shift " +
                "where ShiftStartDateTime between '" + start.ToString("yyyy-MM-dd") + "' AND '" + end.ToString("yyyy-MM-dd") + "'";
            return _db.LoadData<Shift, dynamic>(sql, new { });
        }

        public Task InsertShift(Shift Shift)
        {
            string sql = @"insert into dbo.Shift (EmployeeId, DepartmentId, ShiftStartDateTime, ShiftEndDateTime) 
                        values (@EmployeeId, @DepartmentId, @ShiftStartDateTime, @ShiftEndDateTime);";

            return _db.SaveData(sql, Shift);
        }

        public Task DeleteShift(Shift Shift)
        {
            string sql = @"delete from dbo.Shift where ShiftId = @ShiftId
                         AND EmployeeId = @EmployeeId
                         AND DepartmentId = @DepartmentId;";

            return _db.SaveData(sql, Shift);
        }

        public Task UpdateShift(Shift Shift)
        {
            string sql = @"update dbo.Shift 
                        set ShiftStartDateTime = @ShiftStartDateTime, ShiftEndDateTime = @ShiftEndDateTime
                        where EmployeeId = @EmployeeId 
                        AND ShiftId = @ShiftId
                        AND DepartmentId = @DepartmentId;";

            return _db.SaveData(sql, Shift);
        }

        public Task<List<Employee>> GetEmployeesInRange(int departmentId, string? filter, int offset, int top)
        {
            string sql = @"SELECT * FROM dbo.Employee AS e LEFT JOIN DepartmentEmployee AS de ON e.EmployeeId = de.EmployeeId
                        WHERE CONCAT(FirstName, MiddleName, LastName) LIKE '%" + filter + "%'" +
                        " AND de.DepartmentId = " + departmentId +
                        " ORDER BY FirstName" +
                        " OFFSET " + offset +  " ROWS" +
                        " FETCH NEXT " + top + " ROWS ONLY";
            return _db.LoadData<Employee, dynamic>(sql, new { });
        }

        public Task<int> GetEmployeeCount(int departmentId)
        {
            string sql = @"SELECT COUNT(*) FROM dbo.Employee AS e LEFT JOIN DepartmentEmployee AS de ON e.EmployeeId = de.EmployeeId" +
                        " WHERE de.DepartmentId = " + departmentId;
            return _db.LoadSingleRecord<int, dynamic>(sql, new { });
        }
    }
}
