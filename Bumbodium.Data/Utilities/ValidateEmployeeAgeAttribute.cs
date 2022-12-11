using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bumbodium.Data.Utilities
{
    public class ValidateEmployeeAgeAttribute : ValidationAttribute
    {

        private readonly int _allowedAge;

        public ValidateEmployeeAgeAttribute(int allowedAge)
        {
            _allowedAge = allowedAge;
        }

        public override bool IsValid(object? value)
        {
            return base.IsValid(value);
        }

    }
}
