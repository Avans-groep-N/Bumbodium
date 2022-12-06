using Bumbodium.Data;
using Bumbodium.Data.Interfaces;
using Bumbodium.Data.Repositories;
using Radzen;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Bumbodium.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddTransient<IAvailablityRepo, AvailabilityRepo>();
builder.Services.AddTransient<IShiftRepo, ShiftRepo>();

builder.Services.AddDbContext<BumbodiumContext>(options =>
                options.UseSqlServer("Server=localhost;Database=BumbodiumDB;Trusted_Connection=True;")); //TODO: change back to azure db
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>{
    options.SignIn.RequireConfirmedAccount = false;
    }).AddEntityFrameworkStores<BumbodiumContext>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapBlazorHub();

app.Run();
