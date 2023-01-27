using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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
        private DepartmentRepo _departmentRepo;

        private readonly int Minute = 60;
        private readonly int Hours = 3600;

        public ForecastRepo(BumbodiumContext ctx, DepartmentRepo departmentRepo)
        {
            _ctx = ctx;
            _departmentRepo = departmentRepo;
        }

        public List<Forecast> GetAllInRange(DateTime startDate, DateTime endDate, List<int> departmentIds) => _ctx.Forecast.Where(f => startDate <= f.Date && f.Date >= endDate && departmentIds.Contains(f.DepartmentId)).ToList();

        /*public void CreateForecast(Forecast[] forecasts)
        {
            //TODO Make Country relative to the forecast
            _standards = _ctx.Standards.Where(s => s.Country == Country.Netherlands).ToList();
            var weekprognose = WeekCalEmployes(forecasts);
            foreach (var prognosis in weekprognose)
                _ctx.Forecast.Add(prognosis);
            _ctx.SaveChanges();

        }*/

        private List<Forecast> WeekCalEmployes(Forecast[] forecast)
        {
            List<Forecast> allDepForecasts = new List<Forecast>();

            int percentOfGrantPerDep = (1 / (Enum.GetNames(typeof(DepartmentType)).Length - 1));
            int surfaceAreaOfBranch = _departmentRepo.GetSurfaceOfBranch(1);

            for (int i = 0; i < _amountHoursOpen.Length; i++)
            {
                int amountWorkingHours = DayCalcuWorkHours(workHours:
                    DayCalcuSpiegelen(amountMeters: surfaceAreaOfBranch) +
                    DayCalcuColis(amountColis: forecast[i].AmountExpectedColis) +
                    DayCalcuVakkenVullen(amountColis: forecast[i].AmountExpectedColis),
                    customers: forecast[i].AmountExpectedCustomers,
                    day: i);

                foreach (DepartmentType department in (DepartmentType[])Enum.GetValues(typeof(DepartmentType)))
                {
                    if (department == DepartmentType.Checkout)
                        continue;

                    int test = _departmentRepo.GetSurfaceOfDepartment(1, department);
                    int amountHoursForDepartment = amountWorkingHours * surfaceAreaOfBranch / _departmentRepo.GetSurfaceOfDepartment(1, department);

                    allDepForecasts.Add(new Forecast()
                    {
                        Date = forecast[i].Date,
                        //TODO make branch id relative
                        DepartmentId = _departmentRepo.GetDepartment(department, 1),
                        AmountExpectedColis = forecast[i].AmountExpectedColis,
                        AmountExpectedCustomers = forecast[i].AmountExpectedCustomers,
                        //TODO make branch id relative
                        AmountExpectedHours = amountHoursForDepartment,
                        AmountExpectedEmployees = DayCalcuEmployees(amountHoursForDepartment)
                    });
                }

                amountWorkingHours = DayCalcuKasiere(forecast[i].AmountExpectedCustomers);
                allDepForecasts.Add(new Forecast()
                {

                    Date = forecast[i].Date,
                    //TODO make branch id relative
                    DepartmentId = _departmentRepo.GetDepartment(DepartmentType.Checkout, 1),
                    AmountExpectedColis = forecast[i].AmountExpectedColis,
                    AmountExpectedCustomers = forecast[i].AmountExpectedCustomers,
                    AmountExpectedEmployees = amountWorkingHours / 9,
                    AmountExpectedHours = amountWorkingHours
                });
            }
            return allDepForecasts;
        }

        public int GetAmountOfEmployeesPerDay(DateTime date, int branchId)
        {
            //TODO fix dit je krijg hier 0 return
            var amount = (from f in _ctx.Forecast
                          join d in _ctx.Department on f.DepartmentId equals d.Id
                          where f.Date == date && d.BranchId == branchId
                          select f.AmountExpectedEmployees).Sum();
            return amount;
        }

        //TODO Make Standards relative to country
        private int DayCalcuColis(int amountColis) => _standards.Find(
            s => s.Subject == "Coli" && s.Country == Country.Netherlands).Value * amountColis * Minute;
        private int DayCalcuVakkenVullen(int amountColis) => _standards.Find(
            s => s.Subject == "StockingShelves" && s.Country == Country.Netherlands).Value * amountColis * Minute;
        private int DayCalcuKasiere(int amountCustomers) => amountCustomers / _standards.Find(
            s => s.Subject == "Cashier" && s.Country == Country.Netherlands).Value;
        private int DayCalcuSpiegelen(int amountMeters) => _standards.Find(
            s => s.Subject == "Mirror" && s.Country == Country.Netherlands).Value * amountMeters;
        private int DayCalcuEmployees(int workHours)
        {
            return workHours / 9;
        }

        private int DayCalcuWorkHours(int workHours, int customers, int day)
        {
            int amountOfSecondsToDay = _amountHoursOpen[day] * Hours;
            int workTimeNotWithCustomers = workHours / amountOfSecondsToDay;
            int timeNeededToHelpCustomers = (customers / _standards.Find(
                   s => s.Subject == "Employee" && s.Country == Country.Netherlands).Value) *
                   Hours;

            return (int)(workTimeNotWithCustomers + timeNeededToHelpCustomers) / Hours;
        }

        public List<Forecast> GetForecastOfDate(DateTime startDate, DateTime endDate) => _ctx.Forecast.Where(f => f.Date >= startDate && f.Date <= endDate).ToList();

        public void SaveNewForecast(Forecast forecast)
        {
            _ctx.Forecast.Add(forecast);
            _ctx.SaveChanges();
        }

        public void SaveUpdateForecast(Forecast forecast)
        {
            _ctx.Forecast.Update(forecast);
            _ctx.SaveChanges();
        }



        public void SaveNewForecast(List<Forecast> weakForecast)
        {
            _ctx.Forecast.AddRange(weakForecast);
            _ctx.SaveChanges();
        }

        public void SaveUpdateForecast(List<Forecast> weakForecast)
        {
            _ctx.Forecast.UpdateRange(weakForecast);
            _ctx.SaveChanges();
        }

        public void RemoveForecast(List<Forecast> dbForecast)
        {
            _ctx.RemoveRange(dbForecast);
            _ctx.SaveChanges();
        }

        public List<Forecast> GetForecastInRange(DateTime startDate, DateTime endDate) => _ctx.Forecast.Include(f => f.Department).Where(f => f.Date >= startDate && f.Date <= endDate).ToList();
    }
}
