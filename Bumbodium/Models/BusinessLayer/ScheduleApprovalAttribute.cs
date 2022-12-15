using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Bumbodium.WebApp.Models.BusinessLayer
{
    public class ScheduleApprovalAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if(value == null)
            {
                return false;
            }
            if (isDateAllowed(value))
            {
                return true;
            }
            else
                return false;

        }

        private Boolean isDateAllowed(object value)
        {
            DateTime valueDate = Convert.ToDateTime(value);

            if(DateTime.Now >= valueDate)
            {
                return false;
            }
            else
            return true;
        }

    }
}
