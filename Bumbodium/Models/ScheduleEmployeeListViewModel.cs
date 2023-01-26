using Bumbodium.Data.DBModels;
using System.ComponentModel.DataAnnotations;

namespace Bumbodium.WebApp.Models
{
    public class ScheduleEmployeeListViewModel : IValidatableObject
    {
        public List<Employee> AvailableEmployees { get; set; }
        public string SelectedEmployeeId { get; set; }
        public DepartmentType SelectedDepartment { get; set; }
        public DateTime SelectedDate { get; set; }
        public DateTime SelectedStartTime { get; set; }
        public DateTime SelectedEndTime { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(SelectedDate < DateTime.Now)
            {
                yield return new ValidationResult("Je kunt niet het verleden inplannen");
            }
            
        }
    }
}