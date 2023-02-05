using System.ComponentModel.DataAnnotations;

namespace Bumbodium.WebApp.Models.Utilities.ListValidation
{
    public class ListHasItems : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            List<int> list = value as List<int>;
            if (list == null) return false;
            return list.Any();
        }
    }
}
