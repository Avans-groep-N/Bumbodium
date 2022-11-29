using Bumbodium.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Bumbodium.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BumbodiumContext>(options =>
                options.UseSqlServer("Server=localhost;Database=BumbodiumDB;Trusted_Connection=True;")); //TODO: change back to azure db
        }
    }
}
