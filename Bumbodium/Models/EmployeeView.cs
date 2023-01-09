namespace Bumbodium.WebApp.Models
{
    public class EmployeeView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; internal set; }
    }
}
