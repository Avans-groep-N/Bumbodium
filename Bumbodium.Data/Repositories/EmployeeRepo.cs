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
            return _ctx.Employee
                .Include(e => e.PartOFDepartment)
                .ThenInclude(pod => pod.Department)
                .FirstOrDefault(e => e.EmployeeID == id);
        }

        public IQueryable<Employee> GetEmployees()
        {
            return _ctx.Employee.Include(e => e.PartOFDepartment).ThenInclude(pod => pod.Department).AsQueryable();
        }

        public IEnumerable<Employee> GetEmployeesList(string? nameFilter, int? departmentFilter, int skip, int take)
        {
            IQueryable<Employee> employees = GetEmployeesFiltered(nameFilter, departmentFilter);
            return employees.OrderBy(e => e.FirstName)
                .Skip(skip)
                .Take(take);
        }

        public IQueryable<Employee> GetEmployeesFiltered(string? nameFilter, int? departmentFilter)
        {
            IQueryable<Employee> employees = GetEmployees();
            if (!string.IsNullOrEmpty(nameFilter))
            {
                employees = employees.Where(e => (e.FirstName + " " + e.MiddleName + " " + e.LastName).ToLower().Contains(nameFilter.ToLower()));
            }
            if (departmentFilter > 0)
            {
                employees = employees.Where(e => e.PartOFDepartment.Any(pod => pod.DepartmentId == departmentFilter));
            }
            return employees;
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
            _ctx.SaveChanges();
            AddEmployeeToDepartments(employeeID, departmentIds);
        }
        
        public void AddEmployeeToDepartments(string employeeID, List<int> departmentIds)
        {
            foreach(int id in departmentIds)
            {
                _ctx.DepartmentEmployee.Add(new DepartmentEmployee() { EmployeeId = employeeID, DepartmentId = id });
            }
            _ctx.SaveChanges();
        }

        public List<Employee> GetAllEmployees()
        {
            return _ctx.Employee.Where(e => e.DateOutService == null || e.DateOutService > DateTime.Now).OrderBy(e => e.FirstName).ToList();
        }

    }
}