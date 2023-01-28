using Bumbodium.Data.DBModels;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Bumbodium.WebApp.Models
{
    public class ForecastViewModel
    {
        public DateTime StartOfWeekDate { get; set; }
        public bool WeekBeforeNow { get; private set; }
        public int WeekNr { get; private set; }

        public Dictionary<DateTime, ForecastDay> ForecastWeek { get; }

        public List<DepartmentType> DepartmentTypes { get; set; }

        public ForecastViewModel()
        {
            ForecastWeek = new Dictionary<DateTime, ForecastDay>();
            DepartmentTypes = new List<DepartmentType>();
            foreach (DepartmentType department in Enum.GetValues(typeof(DepartmentType)))
                DepartmentTypes.Add(department);
        }
        public void MakeDictionary(DateTime date)
        {
            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.AddDays(-1);
            }
            StartOfWeekDate = date;

            WeekBeforeNow = StartOfWeekDate <= DateTime.Now.AddDays(-7);

            WeekNr = ISOWeek.GetWeekOfYear(StartOfWeekDate);

            for (int i = 0; i < 7; i++)
            {
                ForecastWeek.Add(StartOfWeekDate.AddDays(i).Date, new ForecastDay());

                foreach (DepartmentType department in Enum.GetValues(typeof(DepartmentType)))
                    ForecastWeek[StartOfWeekDate.AddDays(i).Date].forecastDepartments.Add(department, new ForecastDepartment());
            }
        }

        public int CountTotaalEmplyeesPerDay(DateTime date)
        {
            if (!ForecastWeek.ContainsKey(date.Date))
                return 0;

            int totaal = 0;

            foreach (var value in ForecastWeek[date.Date].forecastDepartments.Values)
                totaal += value.AmountExpectedEmployees;

            return totaal;
        }
        public int CountTotaalHoursPerDay(DateTime date)
        {
            if (!ForecastWeek.ContainsKey(date.Date))
                return 0;

            int totaal = 0;

            foreach (var value in ForecastWeek[date.Date].forecastDepartments.Values)
                totaal += value.AmountExpectedHours;

            return totaal;
        }

        public void AddToDict(DateTime date, ForecastDepartment fD)
        {
            if (ForecastWeek.ContainsKey(date.Date) && ForecastWeek[date.Date].forecastDepartments.ContainsKey(fD.DepartmentType))
            {
                ForecastWeek[date.Date].AddDepartment(fD.DepartmentType, fD);
            }
        }
    }

    public class ForecastDay : IValidatableObject
    {
        public Dictionary<DepartmentType, ForecastDepartment> forecastDepartments { get; }

        public int AmountExpectedCustomers { get; set; }

        public int AmountExpectedColis { get; set; }

        public ForecastDay()
        {
            forecastDepartments = new Dictionary<DepartmentType, ForecastDepartment>();
        }

        public void AddDepartment(DepartmentType department, ForecastDepartment fD)
        {
            if (forecastDepartments.ContainsKey(department))
                forecastDepartments[department] = fD;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (AmountExpectedCustomers < 0)
                yield return new ValidationResult("Negative numbers cannot be added", new[] { "AmountExpectedCustomers" });
            if (AmountExpectedColis < 0)
                yield return new ValidationResult("Negative numbers cannot be added", new[] { "AmountExpectedColis" });
        }
    }

    public class ForecastDepartment : IValidatableObject
    {
        public DepartmentType DepartmentType { get; set; }

        public int AmountExpectedEmployees { get; set; }

        public int AmountExpectedHours { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (AmountExpectedEmployees < 0)
                yield return new ValidationResult("Negative numbers cannot be added", new[] { "AmountExpectedEmployees" });
            if (AmountExpectedHours < 0)
                yield return new ValidationResult("Negative numbers cannot be added", new[] { "AmountExpectedHours" });
        }
    }

}
