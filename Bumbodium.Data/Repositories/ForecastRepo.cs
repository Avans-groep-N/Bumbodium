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
