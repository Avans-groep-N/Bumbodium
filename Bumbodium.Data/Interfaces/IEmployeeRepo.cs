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
        Task<List<Employee>> GetEmployee(IdentityUser user);
        Task InsertEmployee(Employee employee);
        Task<List<IdentityUser>> GetUser(string email);
    }
}