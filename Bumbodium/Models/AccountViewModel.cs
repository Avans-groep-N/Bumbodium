using Bumbodium.Data.DBModels;
using System.ComponentModel.DataAnnotations;

namespace Bumbodium.WebApp.Models
{
    public class AccountViewModel
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public TypeStaff Type { get; set; }
    }

    public enum TypeStaff
    {
        Manager,
        Employee
    }
}
