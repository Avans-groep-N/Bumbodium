using Bumbodium.Data.DBModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bumbodium.Data.Repositories
{
    public class DepartmentRepo
    {
        private BumbodiumContext _ctx;

        public DepartmentRepo(BumbodiumContext ctx)
        {
            _ctx = ctx;
        }

        public int GetDepartment(DepartmentType type, int branchId) => _ctx.Department.FirstOrDefault(d => d.Name == type && d.BranchId == branchId).Id;

        public int GetSurfaceOfDepartment(int branchId, DepartmentType type)
        {
            Department dep = _ctx.Department.First(d => d.BranchId == branchId && d.Name == type);
            if (dep == null)
                return 1;
            return dep.SurfaceAreaInM2;
        }

        public int GetSurfaceOfBranch(int branchId)
        {
            int sum = 0;
            foreach (var dep in _ctx.Department.Where(d => d.BranchId == branchId).ToList())
            {
                if (dep.Name == DepartmentType.Checkout)
                    continue;
                sum += dep.SurfaceAreaInM2;
            }
            return sum;
        }
    }
}
