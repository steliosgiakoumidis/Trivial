using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Trivial.DatabaseAccessLayer;
using Trivial.Entities;
using Trivial.Handlers;
using Trivial.StartUpTask;

namespace Trivial
{
    public class Startup
    {
        private readonly Config _serilogSettings = new Config();

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
            var serilogSection = Configuration.GetSection("Config");
            services.Configure<Config>(opt => serilogSection.Bind(opt));
            serilogSection.Bind(_serilogSettings);

            services.AddDbContext<TrivialContext>(options => options.UseSqlServer(_serilogSettings.ConnectionString));
            services.AddHttpClient();
            services.AddScoped<IHandleTrivialRequest, HandleTrivialRequest>();
            services.AddScoped<IDatabaseAccess, DatabaseAccess>();
            services.AddStartupTask<EnsureDatabase>();
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
