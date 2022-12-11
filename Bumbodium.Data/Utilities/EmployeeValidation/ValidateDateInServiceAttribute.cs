using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Bumbodium.Data.Utilities.EmployeeValidation
{
    public class ValidateDateInServiceAttribute : ValidationAttribute
    {

        private readonly int _minStartingYear;

        public ValidateDateInServiceAttribute(int minStartingYear)
        {
            _minStartingYear = minStartingYear;
        }

        public override bool IsValid(object? value)
        {
            DateTime dateInService = Convert.ToDateTime(value);

            if (dateInService.Year > _minStartingYear)
            {
                return true;
            } else
            {
                return false;
            }
        }

    }
}
