using E_Commerce.Infrastructure.Infrastructure.Bases;
using E_Commerce.Infrastructure.Repositories;
using E_Commerce.Infrastructure.Repositories.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Infrastructure
{
    public static class ModuleInfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IOrderItemRepository, OrderItemRepository>();
            services.AddTransient<IReviewRepository, ReviewRepository>();
            services.AddTransient<IShippingAddressRepository, ShippingAddressRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddTransient<IDeliveryRepository, DeliveryRepository>();
            services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            return services;
        }
    }
}
