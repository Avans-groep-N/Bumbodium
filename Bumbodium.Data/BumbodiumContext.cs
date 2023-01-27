using Bumbodium.Data.DBModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Bumbodium.Data
{
    public class BumbodiumContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public BumbodiumContext(DbContextOptions<BumbodiumContext> options)
            : base(options)
        {
        }
        public DbSet<Presence> Presence { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Availability> Availability { get; set; }
        public DbSet<Shift> Shift { get; set; }
        public DbSet<Branch> Branch { get; set; }
        public DbSet<Standards> Standards { get; set; }
        public DbSet<Forecast> Forecast { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<DepartmentEmployee> DepartmentEmployee { get; set; }
        public DbSet<BranchEmployee> BranchEmployee { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Seed data
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BranchEmployee>()
                .HasKey(bp => new { bp.FiliaalId, bp.EmployeeId });

            modelBuilder.Entity<BranchEmployee>()
                .HasOne(bp => bp.Filiaal)
                .WithMany(p => p.PartOFEmployee)
                .HasForeignKey(bp => bp.FiliaalId);

            modelBuilder.Entity<BranchEmployee>()
                .HasOne(pt => pt.Employee)
                .WithMany(t => t.PartOFFiliaal)
                .HasForeignKey(pt => pt.EmployeeId);

            modelBuilder.Entity<DepartmentEmployee>()
                .HasKey(t => new { t.DepartmentId, t.EmployeeId });

            modelBuilder.Entity<DepartmentEmployee>()
                .HasOne(pt => pt.Department)
                .WithMany(p => p.PartOFEmployee)
                .HasForeignKey(pt => pt.DepartmentId);

            modelBuilder.Entity<DepartmentEmployee>()
                .HasOne(pt => pt.Employee)
                .WithMany(t => t.PartOFDepartment)
                .HasForeignKey(pt => pt.EmployeeId);

            modelBuilder.Entity<Shift>()
                .HasOne(pt => pt.Department)
                .WithMany(t => t.Shifts)
                .HasForeignKey(pt => pt.DepartmentId);

            modelBuilder.Entity<Shift>()
                .HasOne(pt => pt.Employee)
                .WithMany(t => t.Shifts)
                .HasForeignKey(pt => pt.EmployeeId);

            modelBuilder.Entity<Forecast>()
                .HasKey(t => new { t.Date, t.DepartmentId });

            modelBuilder.Entity<Presence>()
                .HasKey(t => new { t.PresenceId, t.EmployeeId });

            #region seedData
            InsertBranchData(modelBuilder);
            InsertDepartmentData(modelBuilder);
            InsertStandardsData(modelBuilder);
            InsertAccountsData(modelBuilder);
            InsertEmployeeData(modelBuilder);
            InsertDepartmentEmployeeData(modelBuilder);
        }

        private static void InsertStandardsData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Standards>().HasData(
                new Standards()
                {
                    Id = 1,
                    Subject = "Coli",
                    Value = 5,
                    Description = "aantal minuten per Coli uitladen.",
                    Country = Country.Netherlands
                },

                new Standards()
                {
                    Id = 2,
                    Subject = "StockingShelves",
                    Value = 30,
                    Description = "aantal minuten Vakken vullen per Coli.",
                    Country = Country.Netherlands
                },

                new Standards()
                {
                    Id = 3,
                    Subject = "Cashier",
                    Value = 30,
                    Description = "1 Kasiere per uur per aantal klanten.",
                    Country = Country.Netherlands
                },

                new Standards()
                {
                    Id = 4,
                    Subject = "Employee",
                    Value = 100,
                    Description = "1 medewerker per customer per uur per aantal klanten.",
                    Country = Country.Netherlands
                },

                new Standards()
                {
                    Id = 5,
                    Subject = "Mirror",
                    Value = 30,
                    Description = "aantal seconde voor medewerker per customer per meter.",
                    Country = Country.Netherlands
                });
        }

        private static void InsertAccountsData(ModelBuilder modelBuilder)
        {
            List<IdentityUser> users = new List<IdentityUser>();
            PasswordHasher<IdentityUser> passwordHasher = new PasswordHasher<IdentityUser>();
            IdentityUser user = new IdentityUser()
            {
                Id = "b74ddd14-6340-4840-95c2-db12554843e5",
                UserName = "Admin",
                Email = "j.vangeest@bumbodium.nl",
                NormalizedEmail = "J.VANGEEST@BUMBODIUM.NL",
                LockoutEnabled = false,
                PhoneNumber = "+31 6 56927484"
            };
            user.PasswordHash = passwordHasher.HashPassword(user, "Password");
            users.Add(user);

            user = new IdentityUser()
            {
                Id = "19f7d479-542a-408b-9016-0561e3e70f65",
                UserName = "Martijs@Martijs.Martijs",
                Email = "Martijs@Martijs.Martijs",
                LockoutEnabled = false,
                PhoneNumber = "Martijs"
            };
            user.PasswordHash = passwordHasher.HashPassword(user, "Martijs");
            users.Add(user);

            user = new IdentityUser()
            {
                Id = "2e835447-b339-4a55-9a74-c0d8449bca5c",
                UserName = "Johnny@vos.nl",
                Email = "Johnny@vos.nl",
                LockoutEnabled = false,
                PhoneNumber = "+31 5"
            };
            user.PasswordHash = passwordHasher.HashPassword(user, "Johnny");
            users.Add(user);

            user = new IdentityUser()
            {
                Id = "44128c29-b648-431e-89f4-7a105f79b00c",
                UserName = "Heinz@vonschmichtelstein.de",
                Email = "Heinz@vonschmichtelstein.de",
                LockoutEnabled = false,
                PhoneNumber = "+49 420 69 7777"
            };
            user.PasswordHash = passwordHasher.HashPassword(user, "Heinz");
            users.Add(user);

            user = new IdentityUser()
            {
                Id = "5782d108-8865-40f8-b3b7-ced82309983f",
                UserName = "Bliksem@martijnshamster.nl",
                Email = "Bliksem@martijnshamster.nl",
                LockoutEnabled = false,
                PhoneNumber = "+31 snel"
            };
            user.PasswordHash = passwordHasher.HashPassword(user, "Bliksem");
            users.Add(user);

            user = new IdentityUser()
            {
                Id = "5989a56b-4d00-4213-9b73-34f80701836b",
                UserName = "Lobbus@kjell.nl",
                Email = "Lobbus@kjell.nl",
                LockoutEnabled = false,
                PhoneNumber = "+31 6 67215943"
            };
            user.PasswordHash = passwordHasher.HashPassword(user, "Lobbus");
            users.Add(user);

            user = new IdentityUser()
            {
                Id = "5fd33111-a002-4ef1-a301-8c4e4e31e20b",
                UserName = "Paula@campina.nl",
                Email = "Paula@campina.nl",
                LockoutEnabled = false,
                PhoneNumber = "+31 612345678"
            };
            user.PasswordHash = passwordHasher.HashPassword(user, "Paula");
            users.Add(user);

            user = new IdentityUser()
            {
                Id = "a20cddd4-9704-439f-94bc-95f4659ce543",
                UserName = "Henkie@Wauwzerz.nl",
                Email = "Henkie@Wauwzerz.nl",
                LockoutEnabled = false,
                PhoneNumber = "ten minste vijf"
            };
            user.PasswordHash = passwordHasher.HashPassword(user, "Henkie");
            users.Add(user);

            user = new IdentityUser()
            {
                Id = "a357223e-5d1e-461e-b1ad-3a8592f548dd",
                UserName = "Katriene@smedensberg.com",
                Email = "Katriene@smedensberg.com",
                LockoutEnabled = false,
                PhoneNumber = "+31 99999999"
            };
            user.PasswordHash = passwordHasher.HashPassword(user, "Katriene");
            users.Add(user);

            user = new IdentityUser()
            {
                Id = "b93d704f-a4ae-413f-a587-0b597bbe6a9f",
                UserName = "Henk@henk.nl",
                Email = "Henk@henk.nl",
                LockoutEnabled = false,
                PhoneNumber = "+31 6666666666"
            };
            user.PasswordHash = passwordHasher.HashPassword(user, "Henk");
            users.Add(user);

            user = new IdentityUser()
            {
                Id = "bdece4e2-3ed9-4008-8878-65884c142394",
                UserName = "Henk@maardanstoer.nl",
                Email = "Henk@maardanstoer.nl",
                LockoutEnabled = false,
                PhoneNumber = "+31 123123123"
            };
            user.PasswordHash = passwordHasher.HashPassword(user, "HenkStoer");
            users.Add(user);

            foreach(IdentityUser userfromlist in users)
            {
                userfromlist.NormalizedUserName = userfromlist.UserName.ToUpper();
            }
            modelBuilder.Entity<IdentityUser>().HasData(users);
        }

        private static void InsertEmployeeData(ModelBuilder modelBuilder)
        {
            List<Employee> employees = new List<Employee>()
            {
                new Employee {
                                EmployeeID = "b74ddd14-6340-4840-95c2-db12554843e5",
                                FirstName = "Jan",
                                MiddleName = "van",
                                LastName = "Geest",
                                Birthdate = new DateTime(1989, 10, 22),
                                PhoneNumber = "+31 6 56927484",
                                Email = "j.vangeest@bumbodium.nl",
                                DateInService = new DateTime(2006, 05, 12),
                                Type = TypeStaff.Manager
                            },
                new Employee {
                                EmployeeID = "2e835447-b339-4a55-9a74-c0d8449bca5c",
                                FirstName = "Johnny",
                                LastName = "Vos",
                                Birthdate = new DateTime(0001, 10, 10),
                                PhoneNumber = "+31 777777777",
                                Email = "Johnny@vos.nl",
                                DateInService = new DateTime(2001, 05, 12),
                                Type = TypeStaff.Manager
                            },
                new Employee {
                                EmployeeID = "19f7d479-542a-408b-9016-0561e3e70f65",
                                FirstName = "Martijs",
                                LastName = "Martijs",
                                Birthdate = new DateTime(0001, 10, 10),
                                PhoneNumber = "Martijs",
                                Email = "Martijs@Martijs.Martijs",
                                DateInService = new DateTime(2001, 05, 12),
                                Type = TypeStaff.Manager
                            },
                new Employee {
                                EmployeeID = "44128c29-b648-431e-89f4-7a105f79b00c",
                                FirstName = "Heinz",
                                MiddleName = "von",
                                LastName = "Schmichtelstein",
                                Birthdate = new DateTime(1975, 10, 09),
                                PhoneNumber = "+49 420 69 7777",
                                Email = "Heinz@vonschmichtelstein.de",
                                DateInService = new DateTime(2009, 05, 12),
                                Type = TypeStaff.Employee
                            },
                new Employee {
                                EmployeeID = "5782d108-8865-40f8-b3b7-ced82309983f",
                                FirstName = "Bliksem",
                                LastName = "Snel",
                                Birthdate = new DateTime(2007, 10, 09),
                                PhoneNumber = "+31 snel",
                                Email = "Bliksem@martijnshamster.nl",
                                DateInService = new DateTime(2009, 05, 12),
                                Type = TypeStaff.Employee
                            },
                new Employee {
                                EmployeeID = "5989a56b-4d00-4213-9b73-34f80701836b",
                                FirstName = "Lobbus",
                                LastName = "Good boy",
                                Birthdate = new DateTime(2005, 10, 09),
                                PhoneNumber = "+31 1684867685",
                                Email = "Lobbus@kjell.nl",
                                DateInService = new DateTime(2009, 05, 12),
                                Type = TypeStaff.Employee
                            },
                new Employee {
                                EmployeeID = "5fd33111-a002-4ef1-a301-8c4e4e31e20b",
                                FirstName = "Paula",
                                LastName = "Campina",
                                Birthdate = new DateTime(1988, 10, 09),
                                PhoneNumber = "+31 612345678",
                                Email = "Paula@campina.nl",
                                DateInService = new DateTime(2020, 05, 12),
                                Type = TypeStaff.Manager
                            },
                new Employee {
                                EmployeeID = "a20cddd4-9704-439f-94bc-95f4659ce543",
                                FirstName = "Henkie",
                                LastName = "T",
                                Birthdate = new DateTime(1957, 10, 09),
                                PhoneNumber = "ten minste vijf",
                                Email = "Henkie@Wauwzerz.nl",
                                DateInService = new DateTime(2019, 05, 12),
                                Type = TypeStaff.Employee
                            },
                new Employee {
                                EmployeeID = "a357223e-5d1e-461e-b1ad-3a8592f548dd",
                                FirstName = "Katriene",
                                LastName = "Smedensberg",
                                Birthdate = new DateTime(1999, 10, 09),
                                PhoneNumber = "+31 99999999",
                                Email = "Katriene@smedensberg.com",
                                DateInService = new DateTime(2014, 05, 12),
                                Type = TypeStaff.Manager
                            },
                new Employee {
                                EmployeeID = "b93d704f-a4ae-413f-a587-0b597bbe6a9f",
                                FirstName = "Henk",
                                LastName = "Henk",
                                Birthdate = new DateTime(1900, 10, 09),
                                PhoneNumber = "+31 6666666666",
                                Email = "Henk@henk.nl",
                                DateInService = new DateTime(2002, 05, 12),
                                Type = TypeStaff.Employee
                            },
                new Employee {
                                EmployeeID = "bdece4e2-3ed9-4008-8878-65884c142394",
                                FirstName = "Henk",
                                MiddleName = "maar dan",
                                LastName = "Stoer",
                                Birthdate = new DateTime(1869, 10, 09),
                                PhoneNumber = "+31 123123123",
                                Email = "Henk@maardanstoer.nl",
                                DateInService = new DateTime(2002, 05, 12),
                                Type = TypeStaff.Employee
                            }
            };
            modelBuilder.Entity<Employee>().HasData(employees);
        }

        private static void InsertDepartmentData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().HasData(
                            new Department() { Id = 1, BranchId = 1, SurfaceAreaInM2 = 50, Name = DepartmentType.Fresh, Description = "Fresh" },
                            new Department() { Id = 2, BranchId = 1, SurfaceAreaInM2 = 140, Name = DepartmentType.Shelves, Description = "Shelves" },
                            new Department() { Id = 3, BranchId = 1, SurfaceAreaInM2 = 90, Name = DepartmentType.Checkout, Description = "Checkout" 
                            });
        }
        private static void InsertDepartmentEmployeeData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DepartmentEmployee>().HasData(
                new DepartmentEmployee() { DepartmentId = 1, EmployeeId = "19f7d479-542a-408b-9016-0561e3e70f65" },
                new DepartmentEmployee() { DepartmentId = 2, EmployeeId = "19f7d479-542a-408b-9016-0561e3e70f65" },
                new DepartmentEmployee() { DepartmentId = 3, EmployeeId = "19f7d479-542a-408b-9016-0561e3e70f65" },
                new DepartmentEmployee() { DepartmentId = 1, EmployeeId = "2e835447-b339-4a55-9a74-c0d8449bca5c" },
                new DepartmentEmployee() { DepartmentId = 2, EmployeeId = "2e835447-b339-4a55-9a74-c0d8449bca5c" },
                new DepartmentEmployee() { DepartmentId = 3, EmployeeId = "2e835447-b339-4a55-9a74-c0d8449bca5c" },
                new DepartmentEmployee() { DepartmentId = 1, EmployeeId = "5782d108-8865-40f8-b3b7-ced82309983f" },
                new DepartmentEmployee() { DepartmentId = 2, EmployeeId = "5782d108-8865-40f8-b3b7-ced82309983f" },
                new DepartmentEmployee() { DepartmentId = 3, EmployeeId = "5782d108-8865-40f8-b3b7-ced82309983f" },
                new DepartmentEmployee() { DepartmentId = 1, EmployeeId = "5989a56b-4d00-4213-9b73-34f80701836b" },
                new DepartmentEmployee() { DepartmentId = 2, EmployeeId = "5989a56b-4d00-4213-9b73-34f80701836b" },
                new DepartmentEmployee() { DepartmentId = 3, EmployeeId = "5989a56b-4d00-4213-9b73-34f80701836b" },
                new DepartmentEmployee() { DepartmentId = 1, EmployeeId = "5fd33111-a002-4ef1-a301-8c4e4e31e20b" },
                new DepartmentEmployee() { DepartmentId = 2, EmployeeId = "5fd33111-a002-4ef1-a301-8c4e4e31e20b" },
                new DepartmentEmployee() { DepartmentId = 3, EmployeeId = "5fd33111-a002-4ef1-a301-8c4e4e31e20b" },
                new DepartmentEmployee() { DepartmentId = 1, EmployeeId = "a20cddd4-9704-439f-94bc-95f4659ce543" },
                new DepartmentEmployee() { DepartmentId = 2, EmployeeId = "44128c29-b648-431e-89f4-7a105f79b00c" },
                new DepartmentEmployee() { DepartmentId = 1, EmployeeId = "a357223e-5d1e-461e-b1ad-3a8592f548dd" },
                new DepartmentEmployee() { DepartmentId = 1, EmployeeId = "b74ddd14-6340-4840-95c2-db12554843e5" },
                new DepartmentEmployee() { DepartmentId = 1, EmployeeId = "b93d704f-a4ae-413f-a587-0b597bbe6a9f" },
                new DepartmentEmployee() { DepartmentId = 1, EmployeeId = "bdece4e2-3ed9-4008-8878-65884c142394" });
        }

        private static void InsertBranchData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Branch>().HasData(
                            new Branch() { Id = 1, City = "Den Bosch", Street = "01", HouseNumber = "1", PostalCode = "0000 AA", Country = Country.Netherlands });
        }
        #endregion
    }
}
