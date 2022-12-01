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
        
        int[] _amountHoursOpen = new int[] { 14, 14, 14, 14, 14, 14, 8 };
        public BumbodiumContext _ctx;

        private readonly int Minute = 60;

        public ForecastRepo(BumbodiumContext ctx)
        {
            _ctx = ctx;
        }

        public List<Forecast> GetAll() => _ctx.Forecast.ToList();

        public void CreateForecast(Forecast[] forecasts)
        {
            _standards = _ctx.Standards.Where(s => s.Country == Country.Netherlands).ToList(); 
            
            _ctx.AddRange(WeekCalEmployes(forecasts));
            _ctx.SaveChanges();

        }

        private List<Forecast> WeekCalEmployes(Forecast[] forecast)
        {
            List<Forecast> allDepForecasts = new List<Forecast>();

            int percentOfGrantPerDep = (1 / Enum.GetNames(typeof(DepartmentType)).Length - 1);

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
                        //TODO Department id inserten
                        AmountExpectedColis = forecast[i].AmountExpectedColis,
                        AmountExpectedCustomers = forecast[i].AmountExpectedCustomers,
                        AmountExpectedEmployees = (int)(amountEmployes * percentOfGrantPerDep + 1)
                    });
                }

                allDepForecasts.Add(new Forecast()
                {
                    Date = forecast[i].Date,
                    //TODO Department id inserten
                    AmountExpectedColis = forecast[i].AmountExpectedColis,
                    AmountExpectedCustomers = forecast[i].AmountExpectedCustomers,
                    AmountExpectedEmployees = CalcuKasiere(forecast[i].AmountExpectedCustomers)
                });
            }
            return allDepForecasts;
        }

        private int DayCalcuColis(int amountColis) => _standards.Find(s => s.Id == 1).Value * amountColis * Minute;
        private int DayCalcuVakkenVullen(int amountColis) => _standards.Find(s => s.Id == 2).Value * amountColis * Minute;
        private int CalcuKasiere(int amountCustomers) => amountCustomers / _standards.Find(s => s.Id == 3).Value;
        private int DayCalcuSpiegelen(int amountMeters) => _standards.Find(s => s.Id == 4).Value * amountMeters;
        private int DayCalcuEmployes(int time, int customers, int day) => customers / (_standards.Find(s => s.Id == 5).Value * _amountHoursOpen[day]);
    }
}
