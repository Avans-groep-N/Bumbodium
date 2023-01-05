using Bumbodium.Data.DBModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bumbodium.Data.Interfaces
{
    public interface IEmployeeRepo
    {
        Employee GetEmployee(string id);
        void InsertEmployee(Employee employee);
        IdentityUser GetUser(string email);
    }
}