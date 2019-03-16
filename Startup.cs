using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LanfeustBridge
{
    using LanfeustBridge.Hubs;
    using LanfeustBridge.Models;
    using LanfeustBridge.Services;

    public class Startup
    {
        private ILogger<Startup> _logger;

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
                // .AddSingleton<IDealsService, SimpleDealsService>()
                .AddSingleton<ITournamentService, DbTournamentsService>()
                // .AddSingleton<ITournamentService, SimpleTournamentsService>()
                .AddSingleton(MovementService.Service);

            services.AddSingleton<IUserStore<User>, UserStoreService>();
            services.AddSingleton<IRoleStore<Role>, UserStoreService>();
            if (!IsDevelopment)
                services.AddSingleton<IEmailSender, SendMail>();
            services.AddIdentity<User, Role>(options => options.Stores.MaxLengthForKeys = 128)
                .AddDefaultUI()
                .AddDefaultTokenProviders();

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
                    // keep a "real" 401/403 for api calls (https://stackoverflow.com/questions/42030137/suppress-redirect-on-api-urls-in-asp-net-core)
                    options.Events.OnRedirectToAccessDenied = ReplaceRedirector(HttpStatusCode.Forbidden, options.Events.OnRedirectToAccessDenied);
                    options.Events.OnRedirectToLogin = ReplaceRedirector(HttpStatusCode.Unauthorized, options.Events.OnRedirectToLogin);
                });
            }

            if (string.IsNullOrEmpty(Configuration["Authentication:Google:ClientId"]))
            {
                _logger.LogInformation("Skipping Google authentication as client-id is not configured");
            }
            else
            {
                services.AddAuthentication().AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                    googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                });
            }

            // no global "RequireAuthenticatedUser", as it interferes with (external) login mechanism
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSignalR();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, RoleManager<Role> roleManager)
        {
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

            EnsureAdminRoleCreated(roleManager).GetAwaiter().GetResult();
        }

        private static async Task EnsureAdminRoleCreated(RoleManager<Role> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new Role("Admin"));
        }

        private static Func<RedirectContext<CookieAuthenticationOptions>, Task> ReplaceRedirector(HttpStatusCode statusCode, Func<RedirectContext<CookieAuthenticationOptions>, Task> existingRedirector)
        {
            return context =>
            {
                if (context.Request.Path.StartsWithSegments("/api"))
                {
                    context.Response.StatusCode = (int)statusCode;
                    return Task.CompletedTask;
                }
                return existingRedirector(context);
            };
        }
    }
}
