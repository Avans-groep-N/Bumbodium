using Microsoft.AspNetCore.Identity;

namespace Bumbodium.WebApp
{
    public class UserAndRoleSeeder
    {
        private static UserManager<IdentityUser> _userManager;
        private static RoleManager<IdentityRole> _roleManager;

        public static void SeedData(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            SeedRoles(roleManager);
            SeedUserRoles(userManager);
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Manager").Result)
            {
                roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "Manager",
                    Id = "0"
                }).Wait();
            }
            if (!roleManager.RoleExistsAsync("Employee").Result)
            {
                roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "Employee",
                    Id = "1"
                }).Wait();
            }
        }

        private static void SeedUserRoles(UserManager<IdentityUser> userManager)
        {
            AddUserToRole("b74ddd14-6340-4840-95c2-db12554843e5", "Manager");
            AddUserToRole("2e835447-b339-4a55-9a74-c0d8449bca5c", "Manager");
            AddUserToRole("19f7d479-542a-408b-9016-0561e3e70f65", "Manager");
            AddUserToRole("44128c29-b648-431e-89f4-7a105f79b00c", "Employee");
            AddUserToRole("5782d108-8865-40f8-b3b7-ced82309983f", "Employee");
            AddUserToRole("5989a56b-4d00-4213-9b73-34f80701836b", "Employee");
            AddUserToRole("5fd33111-a002-4ef1-a301-8c4e4e31e20b", "Manager");
            AddUserToRole("a20cddd4-9704-439f-94bc-95f4659ce543", "Employee");
            AddUserToRole("a357223e-5d1e-461e-b1ad-3a8592f548dd", "Manager");
            AddUserToRole("b93d704f-a4ae-413f-a587-0b597bbe6a9f", "Employee");
            AddUserToRole("bdece4e2-3ed9-4008-8878-65884c142394", "Employee");
        }

        private static void AddUserToRole(string id, string role)
        {
            IdentityUser user = _userManager.FindByIdAsync(id).Result;
            if (!_userManager.GetRolesAsync(user).Result.Any())
            {
                _userManager.AddToRoleAsync(user, role).Wait();
            }
        }
    }
}
