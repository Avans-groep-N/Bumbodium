using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bumbodium.Data.DBModels;
using Microsoft.EntityFrameworkCore;

namespace Bumbodium.Data.Repositories
{
    public class ForecastRepo
    {
        //Dit hoort hier niet maar weet nog niet waar het moet staan dit is even tijdelijk!!!!!!!!
        //Dit mag nog niet naar dev worden gepulled!!

        List<Standards> _standards;
        /*List<Standards> _standards = new List<Standards>() {
                new Standards() {
                    Id = "Coli",
                    Value = 5,
                    Description = "aantal minuten per Coli uitladen.",
                    Country = 1},

                new Standards() {
                    Id= "VakkenVullen",
                    Value = 30,
                    Description = "aantal minuten Vakken vullen per Coli.",
                    Country= "Netherlands"},

                new Standards() {
                    Id = "Kasiere",
                    Value = 30,
                    Description = "1 Kasiere per uur per aantal klanten.",
                    Country = "Netherlands" },

                new Standards() {
                    Id = "Medewerker",
                    Value = 100,
                    Description = "1 medePerCustomer per uur per aantal klanten.",
                    Country = "Netherlands" },

                new Standards() {
                    Id = "Spiegelen",
                    Value = 30,
                    Description = "aantal seconde voor medePerCustomer per meter.",
                    Country = "Netherlands" }

            };*/

        //int[] _amountHoursOpen = new int[] { 14, 14, 14, 14, 14, 14, 8 };
        // werkelijk ^
        int[] _amountHoursOpen = new int[] { 14, 8 };
        // test ^
        public BumbodiumContext _ctx;

        private readonly int Minute = 60;

        public ForecastRepo(BumbodiumContext ctx)
        {
            _ctx = ctx;
        }

        public List<Forecast> GetAll() => _ctx.Forecast.ToList();

        public void CreateForecast(Forecast[] forecasts)
        {
            List<Forecast> departmentforecasts = WeekCalEmployes(forecasts);
            foreach (var dep_forecast in departmentforecasts)
            {
                _ctx.Forecast.Add(dep_forecast);
            }
            //context.AddRange(countries);
            _ctx.SaveChanges();

        }

        private List<Forecast> WeekCalEmployes(Forecast[] forecast)
        {
            List<Forecast> allDepForecasts = new List<Forecast>();

            double percentOfGrantPerDep = 0.125;
            // ^ persentagevoordepartments = (1/ Enum.GetNames(typeof(DepartmentType)).Length -1)

            for (int i = 0; i < _amountHoursOpen.Length; i++)
            {
                int amountEmployes = DayCalcuEmployes(time:
                    //getal van aantal meters klopt niet
                    DayCalcuSpiegelen(amountMeters: 1000) +
                    DayCalcuColis(amountColis: forecast[i].AmountExpectedColis) +
                    DayCalcuVakkenVullen(amountColis: forecast[i].AmountExpectedColis),
                    customers: forecast[i].AmountExpectedCustomers,
                    day: i);

                foreach (DepartmentType department in (DepartmentType[])Enum.GetValues(typeof(DepartmentType)))
                {
                    if (department == DepartmentType.Checkout)
                        continue;

                    allDepForecasts.Add(new Forecast()
                    {
                        Date = forecast[i].Date,
                        DepartmentId = department,
                        AmountExpectedColis = forecast[i].AmountExpectedColis,
                        AmountExpectedCustomers = forecast[i].AmountExpectedCustomers,
                        AmountExpectedEmployees = (int)(amountEmployes * percentOfGrantPerDep + 1)
                    }) ;
                }

                allDepForecasts.Add(new Forecast()
                {
                    Date = forecast[i].Date,
                    DepartmentId = DepartmentType.Checkout,
                    AmountExpectedColis = forecast[i].AmountExpectedColis,
                    AmountExpectedCustomers = forecast[i].AmountExpectedCustomers,
                    AmountExpectedEmployees = CalcuKasiere(forecast[i].AmountExpectedCustomers)
                });
            }
            return allDepForecasts;
        }

        //returns in secondes
        private int DayCalcuColis(int amountColis) => _standards.Find(s => s.Id.Equals("Coli")).Value * amountColis * Minute;
        private int DayCalcuVakkenVullen(int amountColis) => _standards.Find(s => s.Id.Equals("VakkenVullen")).Value * amountColis * Minute;
        private int CalcuKasiere(int amountCustomers) => amountCustomers / _standards.Find(s => s.Id.Equals("Kasiere")).Value;
        private int DayCalcuSpiegelen(int amountMeters) => _standards.Find(s => s.Id.Equals("Spiegelen")).Value * amountMeters;
        private int DayCalcuEmployes(int time, int customers, int day) => customers / (_standards.Find(s => s.Id.Equals("Medewerker")).Value * _amountHoursOpen[day]);
    }
}
