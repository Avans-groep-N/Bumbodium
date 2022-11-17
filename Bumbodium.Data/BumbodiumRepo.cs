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

        public void createNewEmployee(Employee employee)
        {
            _context.Employee.Add(employee);
            _context.SaveChanges();
        }

        public void EditEmployee(Employee employee)
        {
            if (_context.Employee.Contains(employee))
            {
                _context.Employee.Update(employee);
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

    }
}
