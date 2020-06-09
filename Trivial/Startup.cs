using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.IO;
using System.Reflection;
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

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(".\\logs.txt")
                .CreateLogger();

            Configuration = confBuilder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddApplicationInsightsTelemetry();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "User Service", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
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
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Service");
            });
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
