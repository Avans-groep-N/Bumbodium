using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bumbodium.Data.DBModels.EmployeeValidation
{

    public class ValidateDateOutServiceAttribute : ValidationAttribute
    {
        private readonly DateTime _dateInService;

        public ValidateDateOutServiceAttribute(DateTime dateInService)
        {
            _dateInService = dateInService;
        }

/*        public override bool IsValid(object? value)
        {
            DateTime dateOutSerivce = Convert.ToDateTime(value);
        }*/

    }
}
