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

        public string RandomPassword()
        {
            Random random = new Random();
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                {
                    builder.Append(RandomString(1, true));
                }
                else
                {
                    builder.Append(RandomString(1, false));
                }
            }

            string tempPassword = builder.ToString();

            int pos1 = random.Next(0, tempPassword.Length);
            int pos2 = random.Next(0, tempPassword.Length);

            while (pos1 == pos2)
            {
                pos2 = random.Next(0, tempPassword.Length);
            }

            char[] charTempPassword = tempPassword.ToCharArray();

            charTempPassword[pos1] = random.Next(9).ToString()[0];
            charTempPassword[pos2] = random.Next(9).ToString()[0];

            string finalPassword = new string(charTempPassword);

            return finalPassword;
        }

        public void CreateAccount(Employee employee)
        {
            Account account = new Account();
            account.EmployeeId = employee.EmployeeID;
            account.Employee = employee;
            account.Username = employee.FirstName.Substring(0, 1).ToLower() + employee.LastName.ToLower() + employee.EmployeeID.ToString();
            account.Password = RandomPassword();

            if (account != null)
            {
                _context.Accounts.Add(account);
                _context.SaveChanges();
            }
        }

    }
}
