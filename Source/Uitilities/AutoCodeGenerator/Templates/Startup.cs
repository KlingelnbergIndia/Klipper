using NNNN.DataAccess.Implementation;
using NNNN.DataAccess.Interfaces;
using NNNN.Diagnostics;
using NNNN.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace NNNN
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
                    //.ReadFrom.Configuration(Configuration)
                    .WriteTo.Async(a => a.File("NNNN_log_.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true), blockWhenFull: true)
                    .Enrich.FromLogContext()
                    .MinimumLevel.ControlledBy(new LoggingLevelSwitch() { MinimumLevel = LogEventLevel.Information })
                    .Enrich.WithEnvironmentUserName()
                    .Enrich.WithMachineName()
                    .Enrich.WithThreadId()
                    .CreateLogger();

            Log.Information("NNNN starting up...");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddMvcCore()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddAuthorization()
                .AddJsonFormatters();

            services.AddAuthentication("Bearer")
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = "https://localhost:49333";
                //options.RequireHttpsMetadata = false;
                options.ApiName = "NNNN";
            });

            AddMongoDBRelatedServices(services);
        }

        private void AddMongoDBRelatedServices(IServiceCollection services)
        {
            services.Configure<DBConnectionSettings>(options =>
            {
                options.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.Database = Configuration.GetSection("MongoConnection:Database").Value;
            });

            //Add transients for individual type collections here...
            services.AddTransient<IXXXXRepository, XXXXRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors("CorsPolicy");

            loggerFactory.AddSerilog(Log.Logger); //Add serilog here

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseMiddleware<SerilogMiddleware>();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
