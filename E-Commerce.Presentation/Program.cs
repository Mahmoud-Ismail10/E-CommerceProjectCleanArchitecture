using E_Commerce.Core;
using E_Commerce.Core.Filters;
using E_Commerce.Core.Middleware;
using E_Commerce.Domain.Entities.Identity;
using E_Commerce.Infrastructure;
using E_Commerce.Infrastructure.Seeder;
using E_Commerce.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace E_Commerce.Presentation
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region Dependency Injections

            builder.Services
                .AddInfrastructureDependencies()
                .AddServiceDependencies()
                .AddCoreDependencies()
                .AddServiceRegistration(builder.Configuration);
            #endregion

            #region Localization

            builder.Services.AddControllersWithViews();
            builder.Services.AddLocalization(opt =>
            {
                opt.ResourcesPath = "";
            });

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                List<CultureInfo> supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("de-DE"),
                    new CultureInfo("fr-FR"),
                    new CultureInfo("ar-EG")
                };

                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            #endregion

            #region AllowCORS

            var CORS = "_cors";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: CORS,
                                  policy =>
                                  {
                                      policy.AllowAnyHeader();
                                      policy.AllowAnyMethod();
                                      policy.AllowAnyOrigin();
                                  });
            });

            #endregion

            builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            builder.Services.AddTransient<IUrlHelper>(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });

            builder.Services.AddTransient<AuthFilter>();

            var app = builder.Build();

            #region Data Seeder
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
                await RoleSeeder.SeedAsync(roleManager); // Role Seed First
                await UserSeeder.SeedAsync(userManager);
            }
            #endregion

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                //app.MapOpenApi();
            }
            #region Localization Middleware

            var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            #endregion

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }
    }
}