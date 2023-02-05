using Bumbodium.Data.DBModels;
using Microsoft.AspNetCore.Identity;

namespace Bumbodium.Data.Interfaces
{
    public interface IEmployeeRepo
    {
        Employee GetEmployee(string id);
        void InsertEmployee(Employee employee);
        IdentityUser GetUser(string email);
    }
}