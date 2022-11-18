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

        public string RandomPassword(int passWordLength)
        {
            Random random = new Random();
            StringBuilder builder = new StringBuilder();

            int pos1 = random.Next(0, passWordLength);
            int pos2 = random.Next(0, passWordLength);

            while (pos1 == pos2)
            {
                pos2 = random.Next(0, passWordLength);
            }

            for (int i = 0; i < passWordLength; i++)
            {
                if (i == pos1 || i == pos2)
                {
                    builder.Append(random.Next(9).ToString()[0]);
                }
                else if (i % 2 == 0)
                {
                    builder.Append(RandomString(1, true));
                }
                else
                {
                    builder.Append(RandomString(1, false));

                }
            }

            return builder.ToString();

        }

        public void CreateAccount(Employee employee)
        {
            Random random = new Random();

            Account account = new Account();
            account.EmployeeId = employee.EmployeeID;
            account.Employee = employee;
            account.Username = employee.FirstName.Substring(0, 1).ToLower().Replace(" ", "") + employee.LastName.ToLower().Replace(" ", "") + random.Next(9).ToString()[0] + random.Next(9).ToString()[0];
            account.Password = RandomPassword(10);

            if (account != null)
            {
                _context.Accounts.Add(account);
                _context.SaveChanges();
            }
        }

    }
}
