using System.IO;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog;

using LanfeustBridge.Hubs;
using LanfeustBridge.Services;

namespace LanfeustBridge
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<DirectoryService>()
                .AddSingleton<DbService>()
                .AddSingleton<IDealsService, DbDealsService>()
                //.AddSingleton<IDealsService, SimpleDealsService>()
                .AddSingleton<ITournamentService, DbTournamentsService>()
                //.AddSingleton<ITournamentService, SimpleTournamentsService>()
                .AddSingleton(MovementService.Service);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSignalR();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // console and debug logging already set in CreateDefaultBuilder,
            // but we need the DirectoryService for serilog
            var logFilePattern = Path.Combine(app.ApplicationServices.GetService<DirectoryService>().LogDirectory, "LanfeustBridge-{Date}.log");
            var log = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.RollingFile(pathFormat: logFilePattern)
                .CreateLogger();
            loggerFactory.AddSerilog(log);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseSignalR(routes => routes.MapHub<TournamentHub>("/hub/tournament"));
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
