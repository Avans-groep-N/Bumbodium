using System.ComponentModel.DataAnnotations;

namespace Bumbodium.Data.DBModels.EmployeeValidation
{

    public class ValidateDateOutServiceAttribute : ValidationAttribute
    {
        private readonly DateTime _dateInService;

        public ValidateDateOutServiceAttribute(DateTime dateInService)
        {
            _dateInService = dateInService;
        }
    }
}
