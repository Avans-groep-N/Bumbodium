using Bumbodium.Data.DBModels;
using Bumbodium.Data.Repositories;
using Radzen.Blazor.Rendering;
using System.Globalization;

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

        public ForecastWeekViewModel GetForecastWeek(int year, int weekNr)
        {
            //TODO: List of departments with the right branchid
            List<int> departmentIds = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var startDate = ISOWeek.ToDateTime(Convert.ToInt32(year), Convert.ToInt32(weekNr), DayOfWeek.Monday);
            var endDate = startDate.AddDays(7);

            var forecasts = _forcastRepo.GetAllInRange(startDate, endDate, departmentIds);

            var forecastWeek = new ForecastWeekViewModel() { WeekNr = weekNr, YearNr = year };
            FillForecastWeek(forecastWeek, forecasts, startDate);

            return forecastWeek;
        }

        private void FillForecastWeek(ForecastWeekViewModel fw, List<Forecast> forecasts, DateTime startDate)
        {
            for (int dayOff = 0; dayOff < 7; dayOff++)
            {
                List<Forecast> forPerDay = forecasts.Where(f => f.Date.DayOfWeek == startDate.AddDays(dayOff).DayOfWeek).ToList();
                ForecastDayViewModel fi;
                if (forPerDay.Count != 0)
                    fi = FillForecastDay(startDate.AddDays(dayOff), forPerDay);
                else
                    fi = FillEmptyForecastDay(startDate.AddDays(dayOff));
                fw.DaysOfTheWeek[dayOff] = fi;
            }
        }

        private ForecastDayViewModel FillEmptyForecastDay(DateTime dayOfWeek)
        {
            return new ForecastDayViewModel()
            {
                Date = dayOfWeek,
                AmountExpectedColis = 0,
                AmountExpectedCustomers = 0,
                AmountExpectedEmployees = 0,
                AmountExpectedHours = 0,

            };
        }

        private ForecastDayViewModel FillForecastDay(DateTime dayOfWeek, IEnumerable<Forecast> forPerDay)
        {
            return new ForecastDayViewModel()
            {
                Date = dayOfWeek,
                AmountExpectedColis = forPerDay.Sum(f => f.AmountExpectedColis),
                AmountExpectedCustomers = forPerDay.Sum(f => f.AmountExpectedCustomers),
                AmountExpectedEmployees = forPerDay.Sum(f => f.AmountExpectedEmployees),
                AmountExpectedHours = forPerDay.Sum(f => f.AmountExpectedHours),
            };
        }

        public void SaveForecast(ForecastWeekViewModel forecastweek)
        {
            _standards = _standardsRepo.GetAll(Country.Netherlands);
            var startDate = ISOWeek.ToDateTime(forecastweek.YearNr, forecastweek.WeekNr, DayOfWeek.Monday);
            var endDate = startDate.AddDays(7);

            var dbForecast = _forcastRepo.GetForecastOfDate(startDate, endDate);
            if (dbForecast.Count == 0 || dbForecast == null)
                CreateNewForecast(forecastweek);
            else
                UpdateForecastWeek(forecastweek, dbForecast);
        }

        private void UpdateForecastWeek(ForecastWeekViewModel forecastweek, List<Forecast> dbForecast)
        {
            List<Forecast> newForecasts = new List<Forecast>();
            for (int day = 0; day < 7; day++)
            {
                var forecast = forecastweek.DaysOfTheWeek[day];
                newForecasts.AddRange(UpdateForecastDay(forecast, dbForecast.Where(f => f.Date.Date == forecast.Date.Date).ToList(), day));
            }
            _forcastRepo.RemoveForecast(dbForecast);
            _forcastRepo.SaveNewForecast(newForecasts);
        }

        private List<Forecast> UpdateForecastDay(ForecastDayViewModel forecast, List<Forecast> dbForecast, int day)
        {

            if (dbForecast.Count == 0)
                //TODO weghalen
                throw new Exception();


            var branchId = 1;

            if (forecast.AmountExpectedColis != dbForecast.Sum(f => f.AmountExpectedColis) || forecast.AmountExpectedCustomers != dbForecast.Sum(f => f.AmountExpectedCustomers))
            {
                var updatedForecast = new List<Forecast>();
                ForecastDay(forecast, updatedForecast, _departmentRepo.GetSurfaceOfBranch(BranchId), day);

                return updatedForecast;
            }
            else if (forecast.AmountExpectedHours != dbForecast.Sum(f => f.AmountExpectedHours) || forecast.AmountExpectedEmployees != dbForecast.Sum(f => f.AmountExpectedEmployees))
            {

                /* var smallest = dbForecast.First();
                 foreach (var dbFo in dbForecast)
                 {
                     if (dbFo.AmountExpectedEmployees < smallest.AmountExpectedEmployees)
                         smallest = dbFo;

                 }
                 smallest.AmountExpectedEmployees += forecast.AmountExpectedEmployees.Value;

                 smallest = dbForecast.First();
                 foreach (var dbFo in dbForecast)
                 {
                     if (dbFo.AmountExpectedEmployees < smallest.AmountExpectedEmployees)
                         smallest = dbFo;

                 }
                 smallest.AmountExpectedEmployees += forecast.AmountExpectedEmployees*/
            }
            return dbForecast;
        }

        private void CreateNewForecast(ForecastWeekViewModel forecastweek)
        {
            var weekprognose = WeekCalEmployes(forecastweek.DaysOfTheWeek);
            _forcastRepo.SaveNewForecast(weekprognose);
        }

        private List<Forecast> WeekCalEmployes(ForecastDayViewModel[] forecast)
        {
            List<Forecast> allDepForecastsOfWeek = new List<Forecast>();

            int percentOfGrantPerDep = (1 / (Enum.GetNames(typeof(DepartmentType)).Length - 1));
            int surfaceAreaOfBranch = _departmentRepo.GetSurfaceOfBranch(BranchId);

            for (int dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++)
            {
                ForecastDay(forecast[dayOfWeek], allDepForecastsOfWeek, surfaceAreaOfBranch, dayOfWeek);
            }
            return allDepForecastsOfWeek;
        }

        private void ForecastDay(ForecastDayViewModel forecast, List<Forecast> allDepForecasts, int surfaceAreaOfBranch, int day)
        {
            int amountWorkingHours = DayCalcuWorkHours(workHours:
                                DayCalcuSpiegelen(amountMeters: surfaceAreaOfBranch) +
                                DayCalcuColis(amountColis: forecast.AmountExpectedColis) +
                                DayCalcuVakkenVullen(amountColis: forecast.AmountExpectedColis),
                                customers: forecast.AmountExpectedCustomers,
                                day: day);

            foreach (DepartmentType department in (DepartmentType[])Enum.GetValues(typeof(DepartmentType)))
            {
                if (department == DepartmentType.Checkout)
                    continue;

                int test = _departmentRepo.GetSurfaceOfDepartment(1, department);
                int amountHoursForDepartment = amountWorkingHours * surfaceAreaOfBranch / _departmentRepo.GetSurfaceOfDepartment(1, department);

                allDepForecasts.Add(new Forecast()
                {
                    Date = forecast.Date,
                    //TODO make branch id relative
                    DepartmentId = _departmentRepo.GetDepartment(department, 1),
                    AmountExpectedColis = forecast.AmountExpectedColis,
                    AmountExpectedCustomers = forecast.AmountExpectedCustomers,
                    AmountExpectedHours = amountHoursForDepartment,
                    AmountExpectedEmployees = DayCalcuEmployees(amountHoursForDepartment)
                });
            }

            amountWorkingHours = DayCalcuKasiere(forecast.AmountExpectedCustomers);
            allDepForecasts.Add(new Forecast()
            {

                Date = forecast.Date,
                //TODO make branch id relative
                DepartmentId = _departmentRepo.GetDepartment(DepartmentType.Checkout, 1),
                AmountExpectedColis = forecast.AmountExpectedColis,
                AmountExpectedCustomers = forecast.AmountExpectedCustomers,
                AmountExpectedEmployees = amountWorkingHours / 9,
                AmountExpectedHours = amountWorkingHours
            });
        }

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
            int hoursaDay = 14;
            int amountOfSecondsToDay = hoursaDay * 3600;
            int workTimeNotWithCustomers = workHours / amountOfSecondsToDay;
            int timeNeededToHelpCustomers = (customers / _standards.Find(
                   s => s.Subject == "Employee" && s.Country == Country.Netherlands).Value) *
                   3600;

            return (int)(workTimeNotWithCustomers + timeNeededToHelpCustomers) / 3600;
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

        public void ChangeDB(ForecastViewModel ForecastVW)
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

            /*foreach (var forecastDB in forecastsDB)
            {
                ForecastVW.ForecastWeek.TryGetValue(forecastDB.Date.Date, out ForecastDay? value);
                if (value != null)
                {

                }
                else
                {

                }
            }*/
        }
    }
}