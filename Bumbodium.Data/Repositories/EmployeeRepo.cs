using Bumbodium.Data.DBModels;
using Bumbodium.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            return _ctx.Employee.Include(e => e.PartOFDepartment).ThenInclude(pod => pod.Department).FirstOrDefault(e => e.EmployeeID == id);
        }
        public IQueryable<Employee> GetEmployees()
        {
            return _ctx.Employee.AsQueryable();
        }

        public void InsertEmployee(Employee employee)
        {
            _ctx.Employee.Add(employee);
            _ctx.SaveChanges();
        }
        public void UpdateEmployee(Employee employee)
        {
            _ctx.Employee.Update(employee);
            _ctx.SaveChanges();
        }

        public IdentityUser GetUser(string email)
        {
            return _ctx.Users.Where(u => u.Email == email).Single();
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
            AddEmployeeToDepartments(employeeID, departmentIds);
            _ctx.SaveChanges();
        }
        
        public void AddEmployeeToDepartments(string employeeID, List<int> departmentIds)
        {
            foreach(int id in departmentIds)
            {
                _ctx.DepartmentEmployee.Add(new DepartmentEmployee() { EmployeeId = employeeID, DepartmentId = id });
            }
            _ctx.SaveChanges();
        }
    }
}