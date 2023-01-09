using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bumbodium.Data.DBModels.EmployeeValidation
{
    public class ValidateAgeAttribute : ValidationAttribute
    {

        private static readonly int _allowedMinAge = 15;

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

            int age;
            age = DateTime.Now.Year - birthDate.Year;

            if (age > 0)
            {
                age -= Convert.ToInt32(DateTime.Now.Date < birthDate.Date.AddYears(age));
            }
            else
            {
                age = 0;
            }

            if (age >= _allowedMinAge)
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

