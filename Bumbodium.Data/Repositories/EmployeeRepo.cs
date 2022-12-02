using Bumbodium.Data.DBModels;
using Bumbodium.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bumbodium.Data
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly ISqlDataAccess _db;
        public EmployeeRepo(ISqlDataAccess db)
        {
            _db = db;
        }
        public Task<List<Employee>> GetEmployee(IdentityUser user)
        {
            string sql = "SELECT TOP(1) * " +
                "FROM dbo.Employee " +
                "WHERE '" + user.Id + "' = Employee.EmployeeID";
            return _db.LoadData<Employee, dynamic>(sql, new { });
        }
        public Task<List<IdentityUser>> GetUser(string email)
        {
            string sql = $"SELECT TOP(1) * " +
                "FROM dbo.AspNetUsers " +
                "WHERE '" + email + "' = AspNetUsers.Email";
            return _db.LoadData<IdentityUser, dynamic>(sql, new { });
        }
    }
}
