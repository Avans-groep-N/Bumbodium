using Bumbodium.Data.DBModels;
using Bumbodium.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bumbodium.Data
{
    public class CaoInput : ICaoInput
    {
        private readonly Employee _employee;
        private readonly List<Shift> _workedShifts;
        private readonly Shift _plannedShift;
        private readonly int[] _vacationWeeks;

        public CaoInput(Employee employee, List<Shift> workedShifts, Shift plannedShift)
        {
            _employee = employee;
            _workedShifts = workedShifts;
            _plannedShift = plannedShift;
            _vacationWeeks = new[] { 1, 9, 18, 30, 31, 32, 33, 34, 35, 43, 52 };
        }

        public IEnumerable<ValidationResult> ValidateRules()
        {
            //16 or 17
            if (_employee.Age < 18 && _employee.Age >= 16)
            {
                if (MaxAmountHourShift(9))
                {
                    yield return new ValidationResult("Deze werknemer mag maximaal maar 9 uur in één dienst werken", new[] { "MaxAmountHourShift" });
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
                    yield return new ValidationResult("Deze werknemer mag maximaal maar 8 uur in één dienst werken", new[] { "MaxAmountHourShift" });

                if (LatestTime(new TimeSpan(19, 0, 0)))
                    yield return new ValidationResult("Deze werknemer mag niet na 19:00 werken", new[] { "LatestTime" });


                if (_vacationWeeks.Contains<int>(_plannedShift.ShiftStartDateTime.DayOfYear / 7))
                {
                    if (MaxHoursAWeek(40))
                        yield return new ValidationResult("Deze werknemer mag maar 40 uur werken.");
                }
                else
                {
                    if (MaxHoursAWeek(12))
                    yield return new ValidationResult("Deze werknemer mag maar 12 uur werken in de vakantieweken.");
                }
                yield break;
            }

            if (MaxHoursAWeek(60))
                yield return new ValidationResult("Werknemers mogen niet meer dan 60 uur per week werken");
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
        private bool MaxAmountHourShift(int maxHours)
        {
            int schoolHours = 0;
            if (_employee.Availability != null)
            {
                var schoolAvailabilities = _employee.Availability.Where(a => a.Type == AvailabilityType.Schoolhours &&
                                                           a.StartDateTime.Day == _plannedShift.ShiftStartDateTime.Day);
                foreach (var availability in schoolAvailabilities)
                    schoolHours += (availability.EndDateTime - availability.StartDateTime).Hours;
            }

            if ((_plannedShift.ShiftEndDateTime - _plannedShift.ShiftStartDateTime).Hours > (maxHours - schoolHours))
                return true;
            return false;
        }

        //Returns validation result if the average over a month is more than the max amount given
        private bool MaxAverageHoursAWeek(int maxHours)
        {
            int hoursThisMonth = 0;
            var month = _plannedShift.ShiftStartDateTime.Month;
            foreach (var shift in _workedShifts)
            {
                if (shift.ShiftStartDateTime.Month == month)
                    hoursThisMonth += (shift.ShiftEndDateTime - shift.ShiftStartDateTime).Hours;
            }
            if (hoursThisMonth / 7 > maxHours)
                return true;
            return false;
        }

        //Returns validation result if the amount of hours in a week is more than the max amount given
        private bool MaxHoursAWeek(int maxHours)
        {
            int week = _plannedShift.ShiftStartDateTime.DayOfYear / 7;
            var shiftsThisWeek = _workedShifts.Where(s => (s.ShiftStartDateTime.DayOfYear / 7) == week);
            int hoursThisWeek = 0;
            foreach (var shift in shiftsThisWeek)
                hoursThisWeek += (shift.ShiftEndDateTime - shift.ShiftStartDateTime).Hours;

            if (hoursThisWeek > maxHours)
                return true;

            return false;
        }

        //Returns validation result if the amount of shifts this week is more than the max amount given
        private bool MaxShiftsThisWeek(int maxShifts)
        {
            int week = _plannedShift.ShiftStartDateTime.DayOfYear / 7;
            int amountOfShifts = 0;
            foreach (var shift in _workedShifts)
            {
                if (shift.ShiftStartDateTime.DayOfYear / 7 == week)
                    amountOfShifts++;
            }

            if (amountOfShifts > 5)
                return true;
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
