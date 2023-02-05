using Bumbodium.Data.DBModels;
using System.ComponentModel.DataAnnotations;

namespace Bumbodium.WebApp.Models
{
    public class StandardsViewModel
    {
        public Country Country { get; set; }
        public List<Standard> Standards { get; set; }

        public StandardsViewModel()
        {
            Standards = new List<Standard>();
        }

        public void AddToStandards(Standard standard) => Standards.Add(standard);
    }

    public class Standard : IValidatableObject
    {
        public string Description { get; set; }
        public string Subject { get; set; }
        public int Value { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Value < 0)
                yield return new ValidationResult("Negative getallen zijn niet toegestaan.", new[] { nameof(Value) });
        }
    }
}
