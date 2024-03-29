﻿using Bumbodium.Data.DBModels;
using Bumbodium.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
                .Include(e => e.Availability)
                .FirstOrDefault(e => e.EmployeeID == id);
        }

        public IQueryable<Employee> GetEmployees()
        {
            return _ctx.Employee.Include(e => e.PartOFDepartment).ThenInclude(pod => pod.Department).AsQueryable();
        }

        public List<Employee> GetEmployeesList(string? nameFilter, int? departmentFilter, int skip, int take, bool isInactive)
        {
            IQueryable<Employee> employees = GetEmployeesFiltered(nameFilter, departmentFilter, isInactive);
            return employees.OrderBy(e => e.FirstName)
                .Skip(skip)
                .Take(take).ToList();
        }

        public IQueryable<Employee> GetEmployeesFiltered(string? nameFilter, int? departmentFilter, bool isInactive)
        {
            IQueryable<Employee> employees = GetEmployees();
            if (isInactive)
            {
                employees = employees.Where(e => e.DateOutService.HasValue && e.DateOutService < DateTime.Now);
                return employees;
            }

            if (!string.IsNullOrEmpty(nameFilter))
            {
                employees = employees.Where(e => (e.FirstName + " " + e.MiddleName + " " + e.LastName).ToLower().Contains(nameFilter.ToLower()));
            }
            if (departmentFilter > 0)
            {
                employees = employees.Where(e => e.PartOFDepartment.Any(pod => pod.DepartmentId == departmentFilter));
            }
            return employees.Where(e => !e.DateOutService.HasValue || (e.DateOutService.HasValue && e.DateOutService > DateTime.Now));
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
            foreach (int id in departmentIds)
            {
                _ctx.DepartmentEmployee.Add(new DepartmentEmployee() { EmployeeId = employeeID, DepartmentId = id });
            }
            _ctx.SaveChanges();
        }

        public List<Employee> GetAllEmployees()
        {
            return _ctx.Employee.Where(e => e.DateOutService == null || e.DateOutService > DateTime.Now).OrderBy(e => e.FirstName).ToList();
        }

        public IQueryable<Employee> GetAvailableEmployees(int departmentId, DateTime startTime, DateTime endTime)
        {
            IQueryable<Employee> employees = _ctx.Employee.AsQueryable();
            employees = employees.Where(e => e.PartOFDepartment.Any(pod => pod.DepartmentId == (int)(departmentId)));
            employees = employees.Include(e => e.Shifts);
            employees = employees.Where(e => !e.Shifts.Any(s =>
                (s.ShiftStartDateTime > startTime.Date && s.ShiftStartDateTime < startTime.Date.AddHours(23).AddMinutes(59))
            ));

            employees = employees.Include(e => e.Availability);
            employees = employees.Where(e => !e.Availability.Any(a =>
            (a.StartDateTime >= startTime && a.StartDateTime <= endTime) ||
            (a.EndDateTime >= startTime && a.EndDateTime <= endTime) ||
            (a.StartDateTime <= startTime && a.EndDateTime >= endTime)
            ));
            employees = employees.OrderBy(e => e.FirstName);
            return employees;
        }
    }
}