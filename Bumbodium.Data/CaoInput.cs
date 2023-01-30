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

        public List<ValidationResult> ValidateRules()
        {
            var results = new List<ValidationResult>();

            results.Add(MaxAmountHourShift(12));
            results.Add(MaxHoursAWeek(60));

            //16 or 17
            if (_employee.Age < 18 && _employee.Age >= 16)
            {
                results.Add(MaxAmountHourShift(9));
                results.Add(MaxAverageHoursAWeek(40));
            }

            //under 16
            if (_employee.Age < 16)
            {
                results.Add(MaxShiftsThisWeek(5));
                results.Add(MaxAmountHourShift(8));
                results.Add(LatestTime(new TimeOnly(19, 0)));

                foreach(var vacationWeek in _vacationWeeks)
                {
                    if (_plannedShift.ShiftStartDateTime.DayOfYear / 7 == vacationWeek)
                        results.Add(MaxHoursAWeek(40));
                    else
                        results.Add(MaxHoursAWeek(12));
                }
            }

            return results;
        }

        //Calculates the break in minutes
        public TimeSpan CalcBreak()
        {
            TimeSpan breaks = new TimeSpan();
            TimeSpan workingHours = _plannedShift.ShiftEndDateTime - _plannedShift.ShiftStartDateTime;
            int fullBreaks = (int)(workingHours.Minutes / 480) + (int)(workingHours.Minutes / 270); //one break for every 270 minutes/4.5 hours and 480 minutes/8 hours
            for(int i = 0; i < fullBreaks; i++)
            {
                breaks.Add(new TimeSpan(0, 30, 0));
            }

            return breaks;
        }

        #region IndividualRules

        //Returns validation result if the planned shift is longer than the max amount given
        private ValidationResult? MaxAmountHourShift(int maxHours)
        {
            int schoolHours = 0;
            if (_employee.Availability != null)
            {
                var schoolAvailabilities = _employee.Availability.Where(a => a.Type == AvailabilityType.Schoolhours &&
                                                           a.StartDateTime.Day == _plannedShift.ShiftStartDateTime.Day);
                foreach (var availability in schoolAvailabilities)
                    schoolHours += availability.EndDateTime.Hour - availability.StartDateTime.Hour;
            }
            
            if (_plannedShift.ShiftStartDateTime.Hour - _plannedShift.ShiftEndDateTime.Hour > (maxHours - schoolHours))
                return new ValidationResult($"Deze werknemer mag maximaal maar {maxHours} uur in één dienst werken", new[] { "MaxAmountHourShift" });
            return null;
        }

        //Returns validation result if the average over a month is more than the max amount given
        private ValidationResult? MaxAverageHoursAWeek(int maxHours)
        {
            int hoursThisMonth = 0;
            var month = _plannedShift.ShiftStartDateTime.Month;
            foreach(var shift in _workedShifts)
            {
                if(shift.ShiftStartDateTime.Month == month)
                    hoursThisMonth += shift.ShiftEndDateTime.Hour - shift.ShiftStartDateTime.Hour;
            }
            if (hoursThisMonth / 7 > maxHours)
                 return new ValidationResult($"Deze werknemer mag gemiddeld maar {maxHours} uur per week per maand werken", new[] { "MaxAverageHoursAWeek" });
            return null;
        }

        //Returns validation result if the amount of hours in a week is more than the max amount given
        private ValidationResult? MaxHoursAWeek(int maxHours)
        {
            int week = _plannedShift.ShiftStartDateTime.DayOfYear / 7;
            var shiftsThisWeek = _workedShifts.Where(s => (s.ShiftStartDateTime.DayOfYear / 7) == week);
            int hoursThisWeek = 0;
            foreach(var shift in shiftsThisWeek)
                hoursThisWeek += shift.ShiftEndDateTime.Hour - shift.ShiftStartDateTime.Hour;

            if (hoursThisWeek > maxHours)
                return new ValidationResult($"Deze werknemer mag niet langer dan {maxHours} uur in de week werken", new[] { "MaxHoursAWeek" });

            return null;
        }

        //Returns validation result if the amount of shifts this week is more than the max amount given
        private ValidationResult? MaxShiftsThisWeek(int maxShifts)
        {
            int week = _plannedShift.ShiftStartDateTime.DayOfYear / 7;
            int amountOfShifts = 0;
            foreach(var shift in _workedShifts)
            {
                if(shift.ShiftStartDateTime.DayOfYear / 7 == week)
                    amountOfShifts++;
            }

            if(amountOfShifts > 5)
                return new ValidationResult($"Deze werknemer mag maar {maxShifts} keer worden ingepland per week", new[] { "MaxShiftsThisWeek" });
            return null;
        }

        //Returns validation result if the planned end time is later than the time given
        private ValidationResult? LatestTime(TimeOnly time)
        {
            if(_plannedShift.ShiftEndDateTime.TimeOfDay.CompareTo(time) > 0)
            {
                return new ValidationResult($"Deze werknemer mag niet na {time.ToString()} werken", new[] { "LatestTime" });
            }
            return null;
        }

        #endregion

    }
}
