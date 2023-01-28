using Bumbodium.Data.DBModels;
using Bumbodium.Data.Repositories;

namespace Bumbodium.WebApp.Models.Utilities.ForecastValidation
{
    public class BLForecast
    {
        private ForecastRepo _forcastRepo;
        private StandardsRepo _standardsRepo;
        private DepartmentRepo _departmentRepo;
        private List<Standards> _standards;

        private const int Minute = 60;
        private const int BranchId = 1;

        public BLForecast(ForecastRepo forecastRepo, DepartmentRepo departmentRepo, StandardsRepo standardsRepo)
        {
            _forcastRepo = forecastRepo;
            _standardsRepo = standardsRepo;
            _departmentRepo = departmentRepo;
        }

        public ForecastViewModel GetForecast(DateTime date)
        {
            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.AddDays(-1);
            }

            var forecastVW = new ForecastViewModel();
            forecastVW.MakeDictionary(date.Date);

            var forecastsDB = _forcastRepo.GetForecastInRange(date.Date, date.Date.AddDays(7));
            if (forecastsDB == null)
                return forecastVW;

            foreach (var forecast in forecastsDB)
            {
                if (forecastVW.ForecastWeek.ContainsKey(forecast.Date.Date))
                {
                    forecastVW.ForecastWeek[forecast.Date.Date].AmountExpectedCustomers = forecast.AmountExpectedCustomers;
                    forecastVW.ForecastWeek[forecast.Date.Date].AmountExpectedColis = forecast.AmountExpectedColis;

                    forecastVW.AddToDict(forecast.Date.Date, new ForecastDepartment() { DepartmentType = forecast.Department.Name, AmountExpectedEmployees = forecast.AmountExpectedEmployees, AmountExpectedHours = forecast.AmountExpectedHours });
                }
            }

            return forecastVW;
        }

        public void ChangeOutputDB(ForecastViewModel ForecastVW)
        {
            List<Forecast> forecastsDB = _forcastRepo.GetForecastInRange(ForecastVW.StartOfWeekDate.Date, ForecastVW.StartOfWeekDate.Date.AddDays(7));

            foreach (var forecastDay in ForecastVW.ForecastWeek)
            {
                foreach (var forecastDepartment in forecastDay.Value.forecastDepartments)
                {
                    var foreDB = forecastsDB.FirstOrDefault(f => f.Date.Date == forecastDay.Key.Date && f.Department.Name == forecastDepartment.Key);

                    if (foreDB != null)
                    {
                        foreDB.AmountExpectedColis = forecastDay.Value.AmountExpectedColis;
                        foreDB.AmountExpectedCustomers = forecastDay.Value.AmountExpectedCustomers;
                        foreDB.AmountExpectedEmployees = forecastDepartment.Value.AmountExpectedEmployees;
                        foreDB.AmountExpectedHours = forecastDepartment.Value.AmountExpectedHours;
                        _forcastRepo.SaveUpdateForecast(foreDB);
                    }
                    else
                    {
                        //this is only posible if there was not a forecast created
                        _forcastRepo.SaveNewForecast(new Forecast()
                        {
                            Date = forecastDay.Key.Date,
                            DepartmentId = (int)forecastDepartment.Key + 1,
                            AmountExpectedColis = forecastDay.Value.AmountExpectedColis,
                            AmountExpectedCustomers = forecastDay.Value.AmountExpectedCustomers,
                            AmountExpectedEmployees = forecastDepartment.Value.AmountExpectedEmployees,
                            AmountExpectedHours = forecastDepartment.Value.AmountExpectedHours
                        });
                    }
                }
            }
        }

        public void ChangeInputDB(ForecastViewModel ForecastVW)
        {
            List<Forecast> forecastsDB = _forcastRepo.GetForecastInRange(ForecastVW.StartOfWeekDate.Date, ForecastVW.StartOfWeekDate.Date.AddDays(7));

            _standards = _standardsRepo.GetAll(Country.Netherlands);

            foreach (var forecastDay in ForecastVW.ForecastWeek)
            {
                List<Forecast> foreDBs = forecastsDB.Where(f => f.Date.Date == forecastDay.Key.Date).ToList();
                if (forecastDay.Value.AmountExpectedColis == foreDBs.FirstOrDefault()?.AmountExpectedColis && forecastDay.Value.AmountExpectedCustomers == foreDBs.FirstOrDefault()?.AmountExpectedCustomers)
                    continue;

                if (foreDBs != null && foreDBs.Count > 0)
                {
                    _forcastRepo.RemoveForecast(foreDBs);
                }

                List<Forecast> newForecastDB = CreateNewForecastDay(forecastDay.Value, forecastDay.Key);
                _forcastRepo.SaveNewForecast(newForecastDB);

            }
        }

        private List<Forecast> CreateNewForecastDay(ForecastDay forecastDay, DateTime date)
        {
            List<Forecast> forecastsDB = new List<Forecast>();

            int amountWorkingHours = DayCalcuWorkHours(workHours:
                                DayCalcuColis(amountColis: forecastDay.AmountExpectedColis) +
                                DayCalcuVakkenVullen(amountColis: forecastDay.AmountExpectedColis),
                                customers: forecastDay.AmountExpectedCustomers);

            foreach (DepartmentType department in (DepartmentType[])Enum.GetValues(typeof(DepartmentType)))
            {
                if (department == DepartmentType.Checkout)
                    continue;

                int hoursPerDep = amountWorkingHours / Enum.GetValues(typeof(DepartmentType)).Length - 1;
                hoursPerDep = hoursPerDep < 0 ? 0 : hoursPerDep;

                forecastsDB.Add(new Forecast()
                {
                    //id
                    Date = date.Date,
                    DepartmentId = _departmentRepo.GetDepartment(department, 1),

                    //user input
                    AmountExpectedColis = forecastDay.AmountExpectedColis,
                    AmountExpectedCustomers = forecastDay.AmountExpectedCustomers,

                    //calculated output
                    AmountExpectedHours = hoursPerDep,
                    AmountExpectedEmployees = DayCalcuEmployees(hoursPerDep)
                });
            }

            int amoutOfHours = DayCalcuKasiereHours(forecastDay.AmountExpectedCustomers);

            forecastsDB.Add(new Forecast()
            {
                //id
                Date = date.Date,
                DepartmentId = _departmentRepo.GetDepartment(DepartmentType.Checkout, 1),

                //user input
                AmountExpectedColis = forecastDay.AmountExpectedColis,
                AmountExpectedCustomers = forecastDay.AmountExpectedCustomers,

                //calculated output
                AmountExpectedHours = amoutOfHours,
                AmountExpectedEmployees = DayCalcuEmployees(amoutOfHours),

            });
            return forecastsDB;
        }

        

        private int DayCalcuColis(int amountColis) => _standards.Find(
            s => s.Subject == "Coli" && s.Country == Country.Netherlands).Value * amountColis * Minute;
        private int DayCalcuVakkenVullen(int amountColis) => _standards.Find(
            s => s.Subject == "StockingShelves" && s.Country == Country.Netherlands).Value * amountColis * Minute;
        private int DayCalcuKasiereHours(int amountCustomers) => amountCustomers / _standards.Find(
            s => s.Subject == "Cashier" && s.Country == Country.Netherlands).Value;
        private int DayCalcuSpiegelen(int amountMeters) => _standards.Find(
            s => s.Subject == "Mirror" && s.Country == Country.Netherlands).Value * amountMeters;
        private int DayCalcuEmployees(int workHours)
        {
            return workHours / 9;
        }
        private int DayCalcuWorkHours(int workHours, int customers)
        {
            int hoursaDay = 14;
            int amountOfSecondsToDay = hoursaDay * 3600;
            int workTimeNotWithCustomers = workHours / amountOfSecondsToDay;
            int timeNeededToHelpCustomers = (customers / _standards.Find(
                   s => s.Subject == "Employee" && s.Country == Country.Netherlands).Value) *
                   3600;

            return (int)(workTimeNotWithCustomers + timeNeededToHelpCustomers) / 3600;
        }
    }
}