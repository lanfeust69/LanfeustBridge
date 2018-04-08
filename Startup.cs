using System;
using System.IO;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
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
        ILogger<Startup> _logger;

        public Startup(ILogger<Startup> logger, IConfiguration configuration, IHostingEnvironment env)
        {
            _logger = logger;
            Configuration = configuration;
            IsBackendOnly = Configuration.GetValue("BackendOnly", false);
            IsDevelopment = env.IsDevelopment();
        }

        public IConfiguration Configuration { get; }

        public bool IsBackendOnly { get; }

        public bool IsDevelopment { get; }

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

            services.AddSingleton<IUserStore<IdentityUser>, UserStoreService>();
            services.AddSingleton<IRoleStore<IdentityRole>, UserStoreService>();
            services.AddIdentity<IdentityUser, IdentityRole>(options => options.Stores.MaxLengthForKeys = 128)
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            // if (env.IsDevelopment())
            if (IsDevelopment)
            {
                services.Configure<IdentityOptions>(options =>
                {
                    // Password settings
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 3;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredUniqueChars = 1;

                    // Lockout settings
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                    options.Lockout.MaxFailedAccessAttempts = 10;
                    options.Lockout.AllowedForNewUsers = true;

                    // User settings
                    // needs the store to implement IUserEmailStore
                    options.User.RequireUniqueEmail = true;
                });

                services.ConfigureApplicationCookie(options =>
                {
                    // Cookie settings
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    options.LoginPath = "/Identity/Account/Login";
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                    options.SlidingExpiration = true;
                });
            }

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSignalR();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            // console and debug logging already set in CreateDefaultBuilder,
            // but we need the DirectoryService for serilog
            var logFilePattern = Path.Combine(app.ApplicationServices.GetService<DirectoryService>().LogDirectory, "LanfeustBridge-{Date}.log");
            var log = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.RollingFile(pathFormat: logFilePattern)
                .CreateLogger();
            loggerFactory.AddSerilog(log);

            if (IsDevelopment)
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

            if (IsBackendOnly)
            {
                _logger.LogInformation("Skipping serving SPA static files for backend-only use");
            }
            else
            {
                app.UseSpaStaticFiles();
            }

            app.UseAuthentication();

            app.UseSignalR(routes => routes.MapHub<TournamentHub>("/hub/tournament"));
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            if (!IsBackendOnly)
            {
                app.UseSpa(spa =>
                {
                    // To learn more about options for serving an Angular SPA from ASP.NET Core,
                    // see https://go.microsoft.com/fwlink/?linkid=864501

                    spa.Options.SourcePath = "ClientApp";

                    if (IsDevelopment)
                    {
                        spa.UseAngularCliServer(npmScript: "start");
                    }
                });
            }
        }
    }
}
