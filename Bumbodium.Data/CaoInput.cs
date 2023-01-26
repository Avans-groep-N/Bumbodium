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

        public List<CaoValidationResult> ValidateRules()
        {
            var results = new List<CaoValidationResult>();

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
        public int CalcBreak()
        {
            var workingHours = _plannedShift.ShiftEndDateTime.Hour - _plannedShift.ShiftStartDateTime.Hour;
            var fullBreaks = workingHours / 8;
            var halfBreaks = (int)(workingHours / 4.5);
            return ((fullBreaks + halfBreaks) * 30);
        }

        #region IndividualRules

        //Returns validation result if the planned shift is longer than the max amount given
        private CaoValidationResult? MaxAmountHourShift(int maxHours)
        {
            var schoolHours = 0;
            if (_employee.Availability != null)
            {
                var schoolAvailabilities = _employee.Availability.Where(a => a.Type == AvailabilityType.Schoolhours &&
                                                           a.StartDateTime.Day == _plannedShift.ShiftStartDateTime.Day);
                foreach (var availability in schoolAvailabilities)
                    schoolHours += availability.EndDateTime.Hour - availability.StartDateTime.Hour;
            }
            
            if (_plannedShift.ShiftStartDateTime.Hour - _plannedShift.ShiftEndDateTime.Hour > (maxHours - schoolHours))
                return new CaoValidationResult($"Deze werknemer mag maximaal maar {maxHours} uur in één dienst werken", "MaxAmountHourShift");
            return null;
        }

        //Returns validation result if the average over a month is more than the max amount given
        private CaoValidationResult? MaxAverageHoursAWeek(int maxHours)
        {
            int hoursThisMonth = 0;
            var month = _plannedShift.ShiftStartDateTime.Month;
            foreach(var shift in _workedShifts)
            {
                if(shift.ShiftStartDateTime.Month == month)
                    hoursThisMonth += shift.ShiftEndDateTime.Hour - shift.ShiftStartDateTime.Hour;
            }
            if (hoursThisMonth / 7 > maxHours)
                 return new CaoValidationResult($"Deze werknemer mag gemiddeld maar {maxHours} uur per week per maand werken", "MaxAverageHoursAWeek");
            return null;
        }

        //Returns validation result if the amount of hours in a week is more than the max amount given
        private CaoValidationResult? MaxHoursAWeek(int maxHours)
        {
            var week = _plannedShift.ShiftStartDateTime.DayOfYear / 7;
            var shiftsThisWeek = _workedShifts.Where(s => (s.ShiftStartDateTime.DayOfYear / 7) == week);
            var hoursThisWeek = 0;
            foreach(var shift in shiftsThisWeek)
                hoursThisWeek += shift.ShiftEndDateTime.Hour - shift.ShiftStartDateTime.Hour;

            if (hoursThisWeek > maxHours)
                return new CaoValidationResult($"Deze werknemer mag niet langer dan {maxHours} uur in de week werken", "MaxHoursAWeek");

            return null;
        }

        //Returns validation result if the amount of shifts this week is more than the max amount given
        private CaoValidationResult? MaxShiftsThisWeek(int maxShifts)
        {
            var week = _plannedShift.ShiftStartDateTime.DayOfYear / 7;
            var amountOfShifts = 0;
            foreach(var shift in _workedShifts)
            {
                if(shift.ShiftStartDateTime.DayOfYear / 7 == week)
                    amountOfShifts++;
            }

            if(amountOfShifts > 5)
                return new CaoValidationResult($"Deze werknemer mag maar {maxShifts} keer worden ingepland per week", "MaxShiftsThisWeek");
            return null;
        }

        //Returns validation result if the planned end time is later than the time given
        private CaoValidationResult? LatestTime(TimeOnly time)
        {
            if(_plannedShift.ShiftEndDateTime.TimeOfDay.CompareTo(time) > 0)
            {
                return new CaoValidationResult($"Deze werknemer mag niet na {time.ToString()} werken", "LatestTime");
            }
            return null;
        }

        #endregion

    }
}
