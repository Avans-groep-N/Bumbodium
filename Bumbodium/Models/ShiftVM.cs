using Bumbodium.Data;
using Bumbodium.Data.DBModels;
using Radzen.Blazor.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bumbodium.WebApp.Models
{
    public class ShiftVM : IValidatableObject
    {
        public string EmployeeId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //Db doohickery
            var _ctx = (BumbodiumContext)validationContext
                         .GetService(typeof(BumbodiumContext));
            ShiftRepo _shiftRepo = new ShiftRepo(_ctx);
            EmployeeRepo _employeeRepo = new EmployeeRepo(_ctx);
            Employee employee = _employeeRepo.GetEmployee(EmployeeId);

            List<Shift> shiftsThisWeek = _shiftRepo.GetShiftsInRange(StartTime.StartOfWeek(), StartTime.EndOfWeek()).ToList(); 
            var hoursThisWeek = 0;
            foreach (Shift shift in shiftsThisWeek)
                hoursThisWeek += (shift.ShiftStartDateTime.Hour - shift.ShiftEndDateTime.Hour);

            //Verify that shift can only be added if startTime is before EndTime
            if (EndTime.Subtract(StartTime) < TimeSpan.Zero)
                yield return new ValidationResult("End time cannot be before start time", new[] { "StartTime" });

            //Verify that user cannot add a shift on the same day, of the same employee
            DateTime startOfDay = StartTime.Date;
            DateTime endOfDay = startOfDay.AddHours(23).AddMinutes(59);
            List<Shift> possibleShifts = _shiftRepo.GetShiftsInRange(startOfDay, endOfDay)
                .Where(s => s.EmployeeId == employee.EmployeeID)
                .ToList();
            if(possibleShifts.Count > 0)
                yield return new ValidationResult("Cannot add a shift on a day where one already exists", new[] { "ShiftExists" });

            if(employee.Age >= 18)
            {
                //Verify that the user cannot add a shift over 12 hours on 1 day for an employee >=age of 18
                if (StartTime.Hour - EndTime.Hour > 12)
                    yield return new ValidationResult("Cannot add a shift longer than 12 hours", new[] { "ShiftTooLong" });

                //Verify that the user cannot add shifts exceeding 60 hours in 1 month for an employee >= 18 years old
                if (hoursThisWeek > 60)
                    yield return new ValidationResult("Cannot add more than 60 hours a week for an employee", new[] { "TooManyShifts" });
            }

            else
            {
                //Verify that the user cannot add a shift over 9 hours (incl. school hours) on 1 day for an employee < 18 years old
                Availability schoolTime = employee.Availability
                    .Where(a => a.StartDateTime.Date == StartTime.Date && a.Type == AvailabilityType.Schoolhours)
                    .Single();
                var schoolHours = schoolTime.StartDateTime.Hour - schoolTime.EndDateTime.Hour;
                if(StartTime.Hour - EndTime.Hour > (9 - schoolHours))
                    yield return new ValidationResult("Cannot add a shift longer than 9 hours for underage employee", new[] { "ShiftTooLong" });

                //Verify that the user cannot add shifts exceeding 40 hours avergae in 1 month for an employee = 16||17 years old
                List<Shift> shiftsThisMonth = _shiftRepo.GetShiftsInRange(StartTime.StartOfMonth(), StartTime.EndOfMonth()).ToList();
                var hoursThisMonth = 0;
                foreach (Shift shift in shiftsThisWeek)
                    hoursThisMonth += (shift.ShiftStartDateTime.Hour - shift.ShiftEndDateTime.Hour);

                if (hoursThisMonth / 7 > 40)
                    yield return new ValidationResult("Cannot add more than an average of 40 hours a week in 1 month for an underage employee", new[] { "TooManyShifts" });
                
                if (employee.Age < 16)
                {
                    //Verify that the user cannot add over 5 shifts in 1 week for an employee < 16 years old
                    if (shiftsThisWeek.Count >= 5)
                        yield return new ValidationResult("Cannot add more than 5 shifts for an underage employee", new[] { "TooManyShifts" });

                    //Verify that the user cannot add shifts exceeding 12 hours in 1 school week for an employee < 16 years old
                    var vacationWeeks = new[] { 1, 9, 18, 30, 31, 32, 33, 34, 35, 43, 52 };

                    if (!vacationWeeks.Contains(StartTime.DayOfYear / 7))
                    {

                        if (hoursThisWeek > 12)
                            yield return new ValidationResult("Cannot add more than 12 hours a school week for an underage employee", new[] { "TooManyShifts" });
                    }

                    //Verify that the user cannot add shifts exceeding 40 hours in 1 Vacation week for an employee < 16 years old
                    else
                    {
                        if (hoursThisWeek > 40)
                            yield return new ValidationResult("Cannot add more than 40 hours a vacation week for an underage employee", new[] { "TooManyShifts" });
                    }
                }
            }
        }
    }
}