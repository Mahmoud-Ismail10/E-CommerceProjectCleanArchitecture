using E_Commerce.Domain.Entities.Identity;
using E_Commerce.Domain.Helpers;
using E_Commerce.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Text;

namespace E_Commerce.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            // Redis connection
            var redisConnectionString = configuration.GetConnectionString("Redis");
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString!));
            services.AddSingleton(sp => sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase());

            // SQL Server connection
            var sqlConnectionString = configuration.GetConnectionString("CS");
            services.AddDbContext<E_CommerceContext>(options => options.UseSqlServer(sqlConnectionString));

            services.AddIdentity<User, Domain.Entities.Identity.Role>(option =>
            {
                // Password settings.
                option.Password.RequireDigit = true;
                option.Password.RequireLowercase = true;
                option.Password.RequireNonAlphanumeric = true;
                option.Password.RequireUppercase = true;
                option.Password.RequiredLength = 6;
                option.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                option.Lockout.MaxFailedAccessAttempts = 5;
                option.Lockout.AllowedForNewUsers = true;

                // User settings.
                option.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                option.User.RequireUniqueEmail = true;
                option.SignIn.RequireConfirmedEmail = true;

            }).AddEntityFrameworkStores<E_CommerceContext>().AddDefaultTokenProviders();

            // JWT Authentication, Email, and Paymob Payment's GateWay Settings
            var jwtSettings = new JwtSettings();
            var emailSettings = new EmailSettings();
            var paymobSettings = new PaymobSettings();

            configuration.GetSection(nameof(jwtSettings)).Bind(jwtSettings);
            configuration.GetSection(nameof(emailSettings)).Bind(emailSettings);
            configuration.GetSection(nameof(PaymobSettings)).Bind(paymobSettings);

            services.AddSingleton(jwtSettings);
            services.AddSingleton(emailSettings);
            services.AddSingleton(paymobSettings);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(x =>
           {
               x.RequireHttpsMetadata = false;
               x.SaveToken = true;
               x.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = jwtSettings.ValidateIssuer,
                   ValidIssuers = new[] { jwtSettings.Issuer },
                   ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                   ValidAudience = jwtSettings.Audience,
                   ValidateAudience = jwtSettings.ValidateAudience,
                   ValidateLifetime = jwtSettings.ValidateLifeTime,
               };
           });

            //Swagger Gn
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "E-Commerce Project",
                    Version = "v1",
                    Description = "Clean Architecture Project"
                });

                options.EnableAnnotations();

                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                options.MapType<TimeOnly>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "time",
                    Example = new OpenApiString("14:30:00")
                });

                options.MapType<TimeOnly?>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "time",
                    Nullable = true,
                    Example = new OpenApiString("14:30:00")
                });
            });

            // Authorization policies
            services.AddAuthorization(option =>
            {
                option.AddPolicy("EditCustomer", policy =>
                {
                    policy.RequireClaim("Edit Customer", "True");
                });
                option.AddPolicy("GetCustomer", policy =>
                {
                    policy.RequireClaim("Get Customer", "True");
                });
                option.AddPolicy("GetAllCustomer", policy =>
                {
                    policy.RequireClaim("Get All Customer", "True");
                });
                option.AddPolicy("DeleteCustomer", policy =>
                {
                    policy.RequireClaim("Delete Customer", "True");
                });

                option.AddPolicy("CreateAdmin", policy =>
                {
                    policy.RequireClaim("Create Admin", "True");
                });
                option.AddPolicy("EditAdmin", policy =>
                {
                    policy.RequireClaim("Edit Admin", "True");
                });
                option.AddPolicy("GetAdmin", policy =>
                {
                    policy.RequireClaim("Get Admin", "True");
                });
                option.AddPolicy("GetAllAdmin", policy =>
                {
                    policy.RequireClaim("Get All Admin", "True");
                });
                option.AddPolicy("DeleteAdmin", policy =>
                {
                    policy.RequireClaim("Delete Admin", "True");
                });

                option.AddPolicy("CreateEmployee", policy =>
                {
                    policy.RequireClaim("Create Employee", "True");
                });
                option.AddPolicy("EditEmployee", policy =>
                {
                    policy.RequireClaim("Edit Employee", "True");
                });
                option.AddPolicy("GetEmployee", policy =>
                {
                    policy.RequireClaim("Get Employee", "True");
                });
                option.AddPolicy("GetAllEmployee", policy =>
                {
                    policy.RequireClaim("Get All Employee", "True");
                });
                option.AddPolicy("DeleteEmployee", policy =>
                {
                    policy.RequireClaim("Delete Employee", "True");
                });
            });

            return services;
        }
    }
}
