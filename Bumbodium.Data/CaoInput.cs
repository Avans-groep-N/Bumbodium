using Bumbodium.Data.DBModels;
using Bumbodium.Data.Interfaces;
using Radzen.Blazor.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bumbodium.Data
{
    public class CaoInput : ICaoInput
    {
        private readonly Employee _employee;
        private readonly ShiftRepo _shiftRepo;
        private readonly Shift _plannedShift;
        private readonly int[] _vacationWeekNumbers;

        public CaoInput(Employee employee, ShiftRepo shiftRepo, Shift plannedShift)
        {
            _employee = employee;
            _shiftRepo = shiftRepo;
            _plannedShift = plannedShift;
            _vacationWeekNumbers = new[] { 1, 9, 18, 30, 31, 32, 33, 34, 35, 43, 52 };
        }

        public IEnumerable<ValidationResult> ValidateRules()
        {
            //16 or 17
            if (_employee.Age < 18 && _employee.Age >= 16)
            {
                if (MaxAmountHourShift(9))
                {
                    yield return new ValidationResult("Deze werknemer mag maximaal maar 9 uur in één dienst werken (inclusief schooluren)", new[] { "MaxAmountHourShift" });
                }
                if (MaxAverageHoursAWeek(40))
                {
                    yield return new ValidationResult("Deze werknemer mag gemiddeld maar 40 uur per week per maand werken", new[] { "MaxAverageHoursAWeek" });
                }
                yield break;
            }

            //under 16
            if (_employee.Age < 16)
            {
                if (MaxShiftsThisWeek(5))
                    yield return new ValidationResult("Deze werknemer mag maar 5 keer worden ingepland per week", new[] { "MaxShiftsThisWeek" });

                if (MaxAmountHourShift(8))
                    yield return new ValidationResult("Deze werknemer mag maximaal maar 8 uur in één dienst werken (inclusief school)", new[] { "MaxAmountHourShift" });

                if (LatestTime(new TimeSpan(19, 0, 0)))
                    yield return new ValidationResult("Deze werknemer mag niet na 19:00 werken", new[] { "LatestTime" });


                if (_vacationWeekNumbers.Contains(CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(_plannedShift.ShiftStartDateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)))
                {
                    if (MaxHoursAWeek(40))
                        yield return new ValidationResult("Deze werknemer mag maar 40 uur werken deze week");
                }
                else
                {
                    if (MaxHoursAWeek(12))
                        yield return new ValidationResult("Deze werknemer mag maar 12 uur werken deze week");
                }
                yield break;
            }
            if (MaxHoursAMonth(60))
                yield return new ValidationResult("Werknemers mogen niet meer dan 60 uur per maand werken");
            if (MaxAmountHourShift(12))
                yield return new ValidationResult("Werknemers mogen niet langer dan 12 uur werken in een dienst");
        }

        //Calculates the break in minutes
        public TimeSpan CalcBreak()
        {
            TimeSpan breaks = new TimeSpan();
            TimeSpan workingHours = _plannedShift.ShiftEndDateTime - _plannedShift.ShiftStartDateTime;
            int fullBreaks = (int)(workingHours.Minutes / 480) + (int)(workingHours.Minutes / 270); //one break for every 270 minutes/4.5 hours and 480 minutes/8 hours
            for (int i = 0; i < fullBreaks; i++)
            {
                breaks.Add(new TimeSpan(0, 30, 0));
            }

            return breaks;
        }

        #region IndividualRules

        //Returns validation result if the planned shift is longer than the max amount given
        private bool MaxAmountHourShift(double maxHours)
        {
            double schoolHours = 0;
            if (_employee.Availability != null)
            {
                var schoolAvailabilities = _employee.Availability.Where(a => a.Type == AvailabilityType.Schoolhours &&
                                                           a.StartDateTime.Day == _plannedShift.ShiftStartDateTime.Day);
                foreach (Availability availability in schoolAvailabilities)
                    schoolHours += (availability.EndDateTime - availability.StartDateTime).TotalHours;
            }

            if ((_plannedShift.ShiftEndDateTime - _plannedShift.ShiftStartDateTime).TotalHours > (maxHours - schoolHours))
                return true;
            return false;
        }

        //Returns validation result if the average over a month is more than the max amount given
        private bool MaxAverageHoursAWeek(double maxHours)
        {
            double hoursThisMonth = 0;
            foreach (Shift shift in _shiftRepo.GetShiftsInRange(_plannedShift.ShiftStartDateTime.StartOfMonth(),
                _plannedShift.ShiftStartDateTime.EndOfMonth().AddHours(23).AddMinutes(59), _plannedShift.EmployeeId))
            {
                hoursThisMonth += (shift.ShiftEndDateTime - shift.ShiftStartDateTime).TotalHours;
            }
            hoursThisMonth += (_plannedShift.ShiftEndDateTime - _plannedShift.ShiftStartDateTime).TotalHours;
            // hoursThis month divided by the amount of days in the month / 7
            if (hoursThisMonth / (CultureInfo.InvariantCulture.Calendar.GetDaysInMonth(
                _plannedShift.ShiftStartDateTime.Year, 
                _plannedShift.ShiftStartDateTime.Month) / 7) > maxHours)
                return true;
            return false;
        }

        //Returns validation result if the amount of hours in a week is more than the max amount given
        private bool MaxHoursAWeek(double maxHours)
        {
            double hoursThisWeek = 0;
            foreach (Shift shift in _shiftRepo.GetShiftsInRange(_plannedShift.ShiftStartDateTime.StartOfWeek(),
            _plannedShift.ShiftStartDateTime.EndOfWeek(), _plannedShift.EmployeeId))
            {
                hoursThisWeek += (shift.ShiftEndDateTime - shift.ShiftStartDateTime).TotalHours;
            }
            hoursThisWeek += (_plannedShift.ShiftEndDateTime - _plannedShift.ShiftStartDateTime).TotalHours;
            if (hoursThisWeek > maxHours)
                return true;

            return false;
        }
        //Returns validation result if the amount of hours in a week is more than the max amount given
        private bool MaxHoursAMonth(double maxHours)
        {
            double hoursThisMonth = 0;
            foreach (Shift shift in _shiftRepo.GetShiftsInRange(_plannedShift.ShiftStartDateTime.StartOfMonth(),
            _plannedShift.ShiftStartDateTime.EndOfMonth(), _plannedShift.EmployeeId))
            {
                hoursThisMonth += (shift.ShiftEndDateTime - shift.ShiftStartDateTime).TotalHours;
            }
            hoursThisMonth += (_plannedShift.ShiftEndDateTime - _plannedShift.ShiftStartDateTime).TotalHours;
            if (hoursThisMonth > maxHours)
                return true;

            return false;
        }

        //Returns validation result if the amount of shifts this week is more than the max amount given
        private bool MaxShiftsThisWeek(int maxShifts)
        {
            if (_shiftRepo.GetShiftCountInRange(_plannedShift.ShiftStartDateTime.StartOfWeek(),
                _plannedShift.ShiftStartDateTime.EndOfWeek(),
                _plannedShift.EmployeeId) >= maxShifts)
            {
                return true;
            }
            return false;
        }

        //Returns validation result if the planned end time is later than the time given
        private bool LatestTime(TimeSpan time)
        {
            if (_plannedShift.ShiftEndDateTime.TimeOfDay > time)
            {
                return true;
            }
            return false;
        }

        #endregion

    }
}
