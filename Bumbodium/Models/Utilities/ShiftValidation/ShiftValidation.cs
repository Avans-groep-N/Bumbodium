using Bumbodium.Data.DBModels;
using Bumbodium.Data;
using Radzen.Blazor.Rendering;
using System.ComponentModel.DataAnnotations;
using Bumbodium.Data.Interfaces;

namespace Bumbodium.WebApp.Models.Utilities.ShiftValidation
{
    public static class ShiftValidation
    {
        public static IEnumerable<ValidationResult> ValidateShift(IShiftRepo shiftRepo, Shift shift)
        {
            double shiftDuration = CalculateShiftDuration(shift);

            //Verify that shift can only be added if startTime is before EndTime
            if (shiftDuration <= 0)
                yield return new ValidationResult("Eind tijd kan niet hetzelfde zijn als start tijd", new[] { "StartTime" });

            if(shiftDuration < 0.25)
            {
                yield return new ValidationResult("Dienst kan niet korter zijn dan 15 minuten");
            }

            //Verify that user cannot add a shift on the same day, of the same employee
            if (shiftRepo.ShiftExistsInTime(shift.ShiftStartDateTime.Date, shift.ShiftEndDateTime.Date.AddHours(23).AddMinutes(59), shift.EmployeeId))
            {
                yield return new ValidationResult("Er is al een dienst ingepland voor deze dag en medewerker", new[] { "ShiftExists" });
            }
        }

        private static double CalculateShiftDuration(Shift shift)
        {
            return (shift.ShiftEndDateTime - shift.ShiftStartDateTime).TotalHours;
        }
    }
}
