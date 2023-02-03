using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bumbodium.Data.Interfaces
{
    public interface ICaoInput
    {
        public IEnumerable<ValidationResult> ValidateRules();
    }
}
