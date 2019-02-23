using System;
using System.IO;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

using Serilog;

namespace LanfeustBridge
{
    using LanfeustBridge.Services;

    public class Program
    {
        public static int Main(string[] args)
        {
            // we need the DirectoryService before setup of DI...
            var logFile = Path.Combine(new DirectoryService().LogDirectory, "LanfeustBridge-.log");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(path: logFile, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                var builder = WebHost.CreateDefaultBuilder(args)
                    .UseStartup<Startup>()
                    .UseSerilog();

                builder.Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
