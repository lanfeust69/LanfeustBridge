using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json.Serialization;

using Serilog;

using LanfeustBridge.Models;
using LanfeustBridge.Services;

namespace LanfeustBridge
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            //services.AddEntityFramework().AddInMemoryDatabase().AddDbContext<TournamentsContext>(options => options.UseInMemoryDatabase());
            services
                .AddInstance(DirectoryService.Service)
                .AddSingleton<IDealsService, SimpleDealsService>()
                .AddSingleton<ITournamentService, SimpleTournamentsService>();

            services.AddMvc().AddJsonOptions(x => x.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            var logFilePattern = Path.Combine(DirectoryService.Service.LogDirectory, "LanfeustBridge-{Date}.log");
            var log = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.RollingFile(pathFormat: logFilePattern)
                .CreateLogger();
            loggerFactory.AddSerilog(log);

            app.UseIISPlatformHandler();

            // Route all unknown requests to app root
            app.Use(async (context, next) =>
            {
                await next();

                // If there's no available file and the request doesn't contain an extension, we're probably trying to access a page.
                // Rewrite request to use app root
                if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
                {
                    context.Request.Path = "/index.html"; // Angular root page
                    await next();
                }
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMvc();
        }

        // Entry point for the application.
        public static void Main(string[] args) => Microsoft.AspNet.Hosting.WebApplication.Run<Startup>(args);
    }
}
