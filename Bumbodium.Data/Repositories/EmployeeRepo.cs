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
        private BumbodiumContext _ctx;

        public EmployeeRepo(BumbodiumContext ctx)
        {
            _ctx = ctx;
        }
        public Employee GetEmployee(string id)
        {
            return _ctx.Employee.Where(e => e.EmployeeID == id).Single();
        }
        public void InsertEmployee(Employee employee)
        {
            _ctx.Employee.Add(employee);
            _ctx.SaveChanges();
        }

        public IdentityUser GetUser(string email)
        {
            return _ctx.Users.Where(u => u.Email == email).Single();
        }

        public IdentityUser GetUserByName(string name)
        {
            return _ctx.Users.Where(u => u.UserName == name).Single();
        }

        public IdentityUser GetUserById(string id)
        {
            return _ctx.Users.Find(id);
        }
        public void UpdateUser(IdentityUser identityUser)
        {
            _ctx.Users.Update(identityUser);
            _ctx.SaveChanges();
        }

        public void ReplaceDepartmentsOfEmployee(string employeeID, List<int> departmentIds)
        {
            var objectsToDelete = _ctx.DepartmentEmployee.Where(de => de.EmployeeId.Equals(employeeID));
            _ctx.DepartmentEmployee.RemoveRange(objectsToDelete);
            foreach(int id in departmentIds)
            {
                _ctx.DepartmentEmployee.Add(new DepartmentEmployee() { EmployeeId = employeeID, DepartmentId = id});
            }
            _ctx.SaveChanges();
        }

        public string GetEmployeeId(string name)
        {
            return _ctx.Employee.FirstOrDefault(e => e.FirstName == name)?.EmployeeID;
        }

        public List<Employee> GetAllEmployees()
        {
            return _ctx.Employee.Where(e => e.DateOutService == null || e.DateOutService > DateTime.Now).ToList();
        }
    }
}