﻿namespace Bumbodium.WebApp.Models
{
    public class WeekShiftsViewModel
    {

        public List<ShiftVM> WeeklyShifts { get; set; }

        public WeekShiftsViewModel()
        {
            WeeklyShifts = new List<ShiftVM>();
        }

        public void AddShiftVM(ShiftVM s)
        {
            WeeklyShifts.Add(s);
        }

    }
}
