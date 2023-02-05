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

        private const int BranchId = 1;
        private const Country StandardsOfCountry = Country.Netherlands;

        private const int HourSpan = 3600;
        private const int MinuteSpan = 60;
        private const int HoursPerShift = 4;

        public BLForecast(ForecastRepo forecastRepo, DepartmentRepo departmentRepo, StandardsRepo standardsRepo)
        {
            _forcastRepo = forecastRepo;
            _standardsRepo = standardsRepo;
            _departmentRepo = departmentRepo;
            _standards = new List<Standards>();
        }

        public ForecastViewModel GetForecast(DateTime date)
        {
            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.AddDays(-1);
            }

            var forecastVM = new ForecastViewModel();
            forecastVM.MakeDictionary(date.Date);

            var forecastsDB = _forcastRepo.GetForecastInRange(date.Date, date.Date.AddDays(7));
            if (forecastsDB == null)
                return forecastVM;

            foreach (var forecast in forecastsDB)
            {
                if (forecastVM.ForecastWeek.ContainsKey(forecast.Date.Date))
                {
                    forecastVM.ForecastWeek[forecast.Date.Date].AmountExpectedCustomers = forecast.AmountExpectedCustomers;
                    forecastVM.ForecastWeek[forecast.Date.Date].AmountExpectedColis = forecast.AmountExpectedColis;

                    forecastVM.AddToDict(forecast.Date.Date, new ForecastDepartment() { DepartmentType = forecast.Department.Name, AmountExpectedEmployees = forecast.AmountExpectedEmployees, AmountExpectedHours = forecast.AmountExpectedHours });
                }
            }

            return forecastVM;
        }

        public void ChangeOutputDB(ForecastViewModel ForecastVM)
        {
            List<Forecast> forecastsDB = _forcastRepo.GetForecastInRange(ForecastVM.StartOfWeekDate.Date, ForecastVM.StartOfWeekDate.Date.AddDays(7));

            foreach (var forecastDay in ForecastVM.ForecastWeek)
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

        public void ChangeInputDB(ForecastViewModel ForecastVM)
        {
            List<Forecast> forecastsDB = _forcastRepo.GetForecastInRange(ForecastVM.StartOfWeekDate.Date, ForecastVM.StartOfWeekDate.Date.AddDays(7));

            _standards = _standardsRepo.GetAll(Country.Netherlands);

            foreach (var forecastDay in ForecastVM.ForecastWeek)
            {
                List<Forecast> foreDBs = forecastsDB.Where(f => f.Date.Date == forecastDay.Key.Date).ToList();
                if (forecastDay.Value.AmountExpectedColis == foreDBs.FirstOrDefault()?.AmountExpectedColis && forecastDay.Value.AmountExpectedCustomers == foreDBs.FirstOrDefault()?.AmountExpectedCustomers)
                    continue;

                if (foreDBs != null && foreDBs.Count > 0)
                {
                    _forcastRepo.RemoveForecast(foreDBs);
                }

                List<Forecast> newForecastDB = CreateForecastDayForAllDepartments(forecastDay.Value, forecastDay.Key);
                _forcastRepo.CreateForecast(newForecastDB);

            }
        }

        private List<Forecast> CreateForecastDayForAllDepartments(ForecastDay forecastDay, DateTime date)
        {
            List<Forecast> forecastsDB = new List<Forecast>();

            int amountWorkingSecondsShelves = DayClacuShelveSeconds(forecastDay.AmountExpectedColis);
            forecastsDB.Add(
                MakeNewForecast(forecastDay, date, DepartmentType.Shelves, amountWorkingSecondsShelves));

            int amountWorkingSecondsFresh = DayClacuFreshSeconds(forecastDay.AmountExpectedCustomers);
            forecastsDB.Add(
                MakeNewForecast(forecastDay, date, DepartmentType.Fresh, amountWorkingSecondsFresh));

            int amountWorkingSecondsCheckout = DayCalcuKasiereSeconds(forecastDay.AmountExpectedCustomers);
            forecastsDB.Add(
                MakeNewForecast(forecastDay, date, DepartmentType.Checkout, amountWorkingSecondsCheckout));

            return forecastsDB;
        }

        private Forecast MakeNewForecast(ForecastDay forecastDay, DateTime date, DepartmentType departmentType, int amoutOfSecondes)
        {
            return new Forecast()
            {
                //id
                Date = date.Date,
                DepartmentId = _departmentRepo.GetDepartment(departmentType, BranchId),

                //user input
                AmountExpectedColis = forecastDay.AmountExpectedColis,
                AmountExpectedCustomers = forecastDay.AmountExpectedCustomers,

                //calculated output
                AmountExpectedHours = RoundUp(amoutOfSecondes / HourSpan),
                AmountExpectedEmployees = DayCalcuEmployees(RoundUp(amoutOfSecondes / HourSpan))
            };
        }

        private int DayCalcuEmployees(int workHours) => RoundUp(workHours / HoursPerShift);

        private int DayClacuShelveSeconds(int amountColis)
        {
            int colisUnloading = _standards.FirstOrDefault(
            s => s.Subject == "Coli" && s.Country == StandardsOfCountry).Value * amountColis * MinuteSpan;

            int colisStocking = _standards.Find(
            s => s.Subject == "StockingShelves" && s.Country == StandardsOfCountry).Value * amountColis * MinuteSpan;

            return colisStocking + colisUnloading;
        }
        private int DayClacuFreshSeconds(int amountColisCustomers)
        {
            int neededToHelpCustomers = amountColisCustomers / _standards.Find(
                   s => s.Subject == "Employee" && s.Country == StandardsOfCountry).Value * HourSpan;

            int neededSecondsToMirror = _standards.Find(
            s => s.Subject == "Mirror" && s.Country == StandardsOfCountry).Value * _departmentRepo.GetSurfaceOfDepartment(BranchId, DepartmentType.Fresh);

            return neededToHelpCustomers + neededSecondsToMirror;
        }
        private int DayCalcuKasiereSeconds(int amountCustomers) => amountCustomers / _standards.Find(
            s => s.Subject == "Cashier" && s.Country == StandardsOfCountry).Value * HourSpan;

        private int RoundUp(double n)
        {
            return (int)(n + 0.5);
        }
    }
}