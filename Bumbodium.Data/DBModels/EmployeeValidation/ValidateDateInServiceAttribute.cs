using System.ComponentModel.DataAnnotations;

namespace Bumbodium.Data.DBModels.EmployeeValidation
{
    public class ValidateDateInServiceAttribute : ValidationAttribute
    {

        private static readonly int _minStartingYear = 2000;

        public override bool IsValid(object? value)
        {
            DateTime dateInService = Convert.ToDateTime(value);

            if (dateInService.Year > _minStartingYear)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
