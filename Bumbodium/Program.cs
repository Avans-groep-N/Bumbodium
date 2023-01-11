using Bumbodium.Data;
using Bumbodium.Data.Interfaces;
using Bumbodium.Data.Repositories;
using Radzen;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Bumbodium.Data.Repositories;
using Bumbodium.WebApp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Bumbodium.WebApp.Models.Utilities.ExcelExportValidation;
using Bumbodium.WebApp.Models.Utilities.ClockingValidation;
using System.Configuration;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
var configbuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
var config = configbuilder.Build();

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

builder.Services.AddScoped<ForecastRepo>();
builder.Services.AddScoped<DepartmentRepo>();
builder.Services.AddScoped<StandardsRepo>();
builder.Services.AddScoped<PresenceRepo>();
builder.Services.AddScoped<EmployeeRepo>();

builder.Services.AddScoped<BLExcelExport>();
builder.Services.AddScoped<BLClocking>();

builder.Services.AddTransient<IAvailabilityRepo, AvailabilityRepo>();
builder.Services.AddTransient<IShiftRepo, ShiftRepo>();

builder.Services.AddDbContext<BumbodiumContext>(options =>
                options.UseSqlServer(config.GetConnectionString("Default"))); //TODO: change back to azure db
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = false;
    }).AddEntityFrameworkStores<BumbodiumContext>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>(TokenOptions.DefaultProvider);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRequestLocalization();

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("NL-NL");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("NL-NL");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    scope.ServiceProvider.GetRequiredService<BumbodiumContext>().Database.Migrate();
    UserAndRoleSeeder.SeedData(userManager, roleManager);
}
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Index}/{id?}");
app.MapBlazorHub();

app.Run();
