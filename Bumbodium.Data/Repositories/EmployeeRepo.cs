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
        public Task InsertEmployee(Employee employee)
        {
            string sql = @"INSERT INTO dbo.Employee (EmployeeID, FirstName, MiddleName, LastName, Birthdate, PhoneNumber, Email, DateInService, Type) 
                            VALUES (@EmployeeID, @FirstName, @MiddleName, @LastName, @Birthdate, @PhoneNumber, @Email, GETDATE(), 1);";
            return _db.SaveData(sql, employee);
        }

        public Task<List<IdentityUser>> GetUser(string email)
        {
            string sql = $"SELECT TOP(1) * " +
                "FROM dbo.AspNetUsers " +
                "WHERE '" + email + "' = AspNetUsers.Email";
            return _db.LoadData<IdentityUser, dynamic>(sql, new { });
        }

        public Task<List<Employee>> GetEmployees()
        {
            string sql = "SELECT * FROM Employee;";

            return _db.LoadData<Employee, dynamic>(sql, new { });
        }
        public Task<Employee> GetSingleEmployee(string id)
        {
            string sql = @"select * from dbo.Employee where EmployeeID = @EmployeeID;";

            return _db.LoadSingleRecord<Employee, dynamic>(sql, new { });
        }
        public Task DeleteEmployee(Employee employee)
        {
            string sql = @"DELETE FROM dbo.Employee WHERE EmployeeID = @EmployeeID;";

            return _db.SaveData(sql, employee);
        }
        public  Task UpdateEmployee(Employee employee)
        {
            string sql = @"UPDATE dbo.Employee SET FirstName = @FirstName, MiddleName = @MiddleName, LastName = @LastName, Birthdate = @Birthdate, PhoneNumber = @PhoneNumber, Mail = @Mail, DateInService = @DateInService, DateOutService = @DateOutService, Type = @Type WHERE EmployeeID = @EmployeeID;";

            return _db.SaveData(sql, employee);
        }
    }
}