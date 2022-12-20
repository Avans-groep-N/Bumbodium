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
        public Employee GetEmployeeByName(string name)
        {
            return _ctx.Employee.Where(e => e.FullName == name).Single();
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
    }
}