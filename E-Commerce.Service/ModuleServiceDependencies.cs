using E_Commerce.Service.AuthService.Services;
using E_Commerce.Service.AuthService.Services.Contract;
using E_Commerce.Service.Services;
using E_Commerce.Service.Services.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using X.Paymob.CashIn;

namespace E_Commerce.Service
{
    public static class ModuleServiceDependencies
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IOrderItemService, OrderItemService>();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddTransient<IPaymobService, PaymobService>();
            services.AddTransient<IShippingAddressService, ShippingAddressService>();
            services.AddTransient<IApplicationUserService, ApplicationUserService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IAuthorizationService, AuthorizationService>();
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IEmailsService, EmailsService>();
            services.AddTransient<IFileService, FileService>();
            services.AddScoped<ICartService, CartService>();
            services.AddPaymobCashIn(options =>
            {
                options.ApiKey = configuration["Paymob:ApiKey"];
                options.Hmac = configuration["Paymob:HMAC"];
            });
            return services;
        }
    }
}
