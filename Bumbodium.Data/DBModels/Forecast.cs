using System.ComponentModel.DataAnnotations;

namespace Bumbodium.Data.DBModels
{
    public class Forecast
    {
        [Key]
        public DateTime Date { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        [Required]
        public int AmountExpectedEmployees { get; set; }
        
        [Required]
        public int AmountExpectedCustomers { get; set; }
        
        [Required]
        public int AmountExpectedColis { get; set; }
    }
}
