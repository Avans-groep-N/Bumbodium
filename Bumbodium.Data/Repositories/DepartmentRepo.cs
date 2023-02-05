using Bumbodium.Data.DBModels;

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

        public Department GetDepartmentById(int id) => _ctx.Department.Where(d => d.Id == id).Single();

        public IEnumerable<Department> GetAllDepartments() => _ctx.Department;

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

        public void AddEmployeeToDepartment(string EmployeeId, int DepartmentId)
        {
            var employee = new DepartmentEmployee()
            {
                EmployeeId = EmployeeId,
                DepartmentId = DepartmentId,
                WorkFunction = 0
            };
            _ctx.DepartmentEmployee.Add(employee);
            _ctx.SaveChanges();
        }
    }
}
