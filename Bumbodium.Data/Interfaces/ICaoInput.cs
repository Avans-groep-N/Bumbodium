using System.ComponentModel.DataAnnotations;

namespace Bumbodium.Data.Interfaces
{
    public interface ICaoInput
    {
        public IEnumerable<ValidationResult> ValidateRules();
    }
}
