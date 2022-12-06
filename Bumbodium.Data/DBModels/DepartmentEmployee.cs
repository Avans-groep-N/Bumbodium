namespace Bumbodium.Data.DBModels
{
    public class DepartmentEmployee
    {
        public string EmployeeId { get; set; }
        public int DepartmentId { get; set; }

        public Department Department { get; set; }
        public Employee Employee { get; set; }

        public WorkFunction WorkFunction { get; set; }
    }

    public enum WorkFunction
    {
        Fresh,
        stockClerk,
        TeamLeader,
        Butcher
    }
}
