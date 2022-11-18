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

        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public string RandomPassword(int size)
        {
            Random random = new Random();

            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(random.Next(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }

        public void CreateAccount(Employee employee)
        {
            Account account = new Account();
            account.EmployeeId = employee.EmployeeID;
            account.Employee = employee;
            account.Username = employee.FirstName.Substring(0, 1).ToLower() + employee.LastName.ToLower() + employee.EmployeeID.ToString();
            account.Password = RandomPassword(12);

            if (account != null)
            {
                _context.Accounts.Add(account);
                _context.SaveChanges();
            }
        }

    }
}
