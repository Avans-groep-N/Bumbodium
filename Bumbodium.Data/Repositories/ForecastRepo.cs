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
        private List<Standards> _standards;
        
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
            //_standards = _ctx.Standards.Where(s => s.Id == "Netherlands").ToList();

            //List<Forecast> departmentforecasts = ;
            
            _ctx.AddRange(WeekCalEmployes(forecasts));
            _ctx.SaveChanges();

        }

        private List<Forecast> WeekCalEmployes(Forecast[] forecast)
        {
            List<Forecast> allDepForecasts = new List<Forecast>();

            //percentage of departments has been abstracted for convenience. real equation is "= (1/ Enum.GetNames(typeof(DepartmentType)).Length -1)"
            //TODO: add the correct equation for the percentage of departments
            double percentOfGrantPerDep = 0.125;

            for (int i = 0; i < _amountHoursOpen.Length; i++)
            {
                int amountEmployes = DayCalcuEmployes(time:
                    //TODO: add the correct amount of metres
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
                        //DepartmentId = department,
                        AmountExpectedColis = forecast[i].AmountExpectedColis,
                        AmountExpectedCustomers = forecast[i].AmountExpectedCustomers,
                        AmountExpectedEmployees = (int)(amountEmployes * percentOfGrantPerDep + 1)
                    });
                }

                allDepForecasts.Add(new Forecast()
                {
                    Date = forecast[i].Date,
                    //DepartmentId = DepartmentType.Checkout,
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
