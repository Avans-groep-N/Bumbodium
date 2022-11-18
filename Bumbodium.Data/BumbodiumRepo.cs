using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bumbodium.Data
{

    public class BumbodiumRepo
    {
        BumbodiumContext _context = new BumbodiumContext();

        public void CreateEmployee(Employee employee)
        {
            _context.Employee.Add(employee);
            _context.SaveChanges();
        }

        public void EditEmployee(Employee employee)
        {
            if (_context.Employee.Contains(employee))
            {
                _context.Attach(employee);
                _context.Employee.Update(employee);
                _context.SaveChanges();
            }

            _context.SaveChanges();
        }

        public void DeleteEmployee(Employee employee)
        {
            if (_context.Employee.Contains(employee))
            {
                _context.Employee.Remove(employee);
                _context.SaveChanges();
            }
        }

        public List<Employee> GetEmployees()
        {
            return _context.Employee.ToList();
        }

        public Employee GetEmployee(int id)
        {
            return _context.Employee.Where(e => e.EmployeeID == id).FirstOrDefault();
        }

        public void createAccount(Employee employee)
        {
            Account _account = new Account();
            _account.Employee = employee;
            _account.Username = employee.FirstName.Substring(0, 1) + employee.LastName + employee.EmployeeID.ToString();
        }

    }
}
