using Bumbodium.Data.DBModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Bumbodium.Data.Repositories
{
    public class ForecastRepo
    {
        public BumbodiumContext _ctx;

        public ForecastRepo(BumbodiumContext ctx, DepartmentRepo departmentRepo)
        {
            _ctx = ctx;
        }

        public List<Forecast> GetAllInRange(DateTime startDate, DateTime endDate, List<int> departmentIds) => _ctx.Forecast.Where(f => startDate <= f.Date && f.Date >= endDate && departmentIds.Contains(f.DepartmentId)).ToList();

        public List<Forecast> GetForecastOfDate(DateTime startDate, DateTime endDate) => _ctx.Forecast.Where(f => f.Date >= startDate && f.Date <= endDate).ToList();

        public int GetNeededHoursOfDepartmentOnDate(DateTime date, int departmentId)
        {
            int hoursNeeded = 0;
            var forecast = _ctx.Forecast
                .Where(f => f.Date == date && f.DepartmentId == departmentId)
                .FirstOrDefault();
            if (forecast != null)
            {
                hoursNeeded = forecast.AmountExpectedHours;
            }
            return hoursNeeded;
        }

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

        public void CreateForecast(List<Forecast> weakForecast)
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
