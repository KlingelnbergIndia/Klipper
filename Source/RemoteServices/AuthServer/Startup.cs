using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Common.Logging;
using IdentityServer4.Stores;
using AuthServer.Configuration;

namespace AuthServer
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("ClientApiConfig.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
                    .WriteTo.Async(a => a.File("AuthSever_log_.log", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true), blockWhenFull: true)
                    .Enrich.FromLogContext()
                    .MinimumLevel.ControlledBy(new LoggingLevelSwitch() { MinimumLevel = LogEventLevel.Warning })
                    .Enrich.WithEnvironmentUserName()
                    .Enrich.WithMachineName()
                    .Enrich.WithThreadId()
                    .CreateLogger();

            Log.Information("AuthServer starting up...");
        }

        public IConfiguration Configuration { get; }

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
            .AddJsonFormatters();

            services.AddIdentityServer(
                    options =>
                    {
                        options.Events.RaiseSuccessEvents = true;
                        options.Events.RaiseFailureEvents = true;
                        options.Events.RaiseErrorEvents = true;
                    }
                )
                .AddDeveloperSigningCredential()
                .AddJwtBearerClientAuthentication()
                .AddInMemoryApiResources(ConfigReader.GetApiResources(Configuration))
                .AddInMemoryClients(ConfigReader.GetClients(Configuration));
        }

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
            app.UseMiddleware<SerilogMiddleware>();
            app.UseIdentityServer();
            app.UseMvc();
        }
    }
}
