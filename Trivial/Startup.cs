using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trivial.DatabaseAccessLayer;
using Trivial.Handlers;
using Microsoft.Extensions.Hosting;
using Trivial.Entities;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace Trivial
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var confBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            Configuration = confBuilder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //services.AddDbContext<TrivialContext>(options => options.UseSqlServer("Server=192.168.0.29,1433;Database=Trivial;User Id=SA;Password=Oresti18;"));
            services.AddDbContext<TrivialContext>(options => options.UseSqlServer("Server=stelios\\sqlexpress;Database=Trivial;Trusted_Connection=True;"));
            services.AddHttpClient();
            services.Configure<Config>(opt => Configuration.GetSection("Config").Bind(opt));
            services.AddScoped<IHandleTrivialRequest, HandleTrivialRequest>();
            services.AddScoped<IDatabaseAccess, DatabaseAccess>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, 
            IConfiguration config)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
