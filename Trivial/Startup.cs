using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trivial.DatabaseAccessLayer;
using Trivial.Handlers;
using Trivial.Entities;

namespace Trivial
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
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
            //services.AddDbContext<TrivialContext>(options => options.UseSqlServer("Server=192.168.0.29,1433;Database=Trivial;User Id=SA;Password=Oresti18;"));
            services.AddDbContext<TrivialContext>(options => options.UseSqlServer("Server=stelios\\sqlexpress;Database=Trivial;Trusted_Connection=True;"));
            services.AddHttpClient();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.Configure<Config>(opt => Configuration.GetSection("Config").Bind(opt));
            services.AddScoped<IHandleTrivialRequest, HandleTrivialRequest>();
            services.AddScoped<IDatabaseAccess, DatabaseAccess>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
