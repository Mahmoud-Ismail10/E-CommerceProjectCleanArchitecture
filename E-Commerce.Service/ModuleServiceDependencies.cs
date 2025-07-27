using E_Commerce.Service.AuthService.Services;
using E_Commerce.Service.AuthService.Services.Contract;
using E_Commerce.Service.Services;
using E_Commerce.Service.Services.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Service
{
    public static class ModuleServiceDependencies
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
        {
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddTransient<IApplicationUserService, ApplicationUserService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IAuthorizationService, AuthorizationService>();
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IEmailsService, EmailsService>();
            services.AddTransient<IFileService, FileService>();

            return services;
        }
    }
}
