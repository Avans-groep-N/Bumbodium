using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bumbodium.Data.Utilities.EmployeeValidation
{
    public class ValidateAgeAttribute : ValidationAttribute
    {

        private static readonly int _allowedMinAge = 15;
        private static readonly int _allowedMaxAge = 67;

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            if (IsAgeAllowed(value))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool IsAgeAllowed(object value)
        {

            DateTime birthDate = Convert.ToDateTime(value);

            int age = DateTime.Now.Year - birthDate.Year;

            if (age > _allowedMinAge && age < _allowedMaxAge)
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

