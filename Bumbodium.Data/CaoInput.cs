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

        public CaoInput(Employee employee, List<Shift> workedShifts, Shift plannedShift)
        {
            _employee = employee;
            _workedShifts = workedShifts;
            _plannedShift = plannedShift;
        }

        public List<CaoValidationResult> ValidateRules()
        {
            var results = new List<CaoValidationResult>();
            results.Add(SameDaySameTime());

            //18 or older
            if(_employee.Age >= 18)
            {
                results.Add(MaxAmountHourShift(12));
                results.Add(MaxAmountHoursAMonth(60));
            }

            //18 or under
            if(_employee.Age < 18)
            {
                results.Add(MaxAmountHourShift(9));
                results.Add(MaxAmountHoursAMonth(40));

                //16 or under
                if (_employee.Age < 16)
                {
                    results.Add(MaxShiftsThisWeek(5));
                }
            }

            return results;
        }

        #region IndividualRules
        private CaoValidationResult? SameDaySameTime()
        {
            //Verify that user cannot add a shift on the same day, of the same employee
            foreach(var shift in _workedShifts)
                if(shift.ShiftStartDateTime == _plannedShift.ShiftStartDateTime)
                    return new CaoValidationResult("Cannot add a shift on a day where one already exists", "SameDaySameTime");
            return null;
        }

        private CaoValidationResult? MaxAmountHourShift(int maxHours)
        {
            //Verify that the user cannot add a shift over 12 hours on 1 day for an employee >=age of 18
            if (_plannedShift.ShiftStartDateTime.Hour - _plannedShift.ShiftEndDateTime.Hour > maxHours)
                return new CaoValidationResult($"Cannot add a shift longer than {maxHours} hours for this employee", "MaxAmountHourShift");
            return null;
        }

        private CaoValidationResult? MaxAmountHoursAMonth(int maxHours)
        {
            //Verify that the user cannot add shifts exceeding 60 hours in 1 month for an employee >= 18 years old
            int hoursThisMonth = 0;
            var month = _plannedShift.ShiftStartDateTime.Month;
            foreach(var shift in _workedShifts)
            {
                if(shift.ShiftStartDateTime.Month == month)
                    hoursThisMonth += shift.ShiftEndDateTime.Hour - shift.ShiftStartDateTime.Hour;
            }
            if (hoursThisMonth / 7 > maxHours)
                 return new CaoValidationResult($"Cannot add more than {maxHours} hours a week for this employee", "MaxAmountHoursAMonth");
            return null;
        }

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
                return new CaoValidationResult($"Cannot add more than {maxShifts} shifts for an underage employee", "MaxShiftsThisWeek");
            return null;
        }

        #endregion

    }
}
