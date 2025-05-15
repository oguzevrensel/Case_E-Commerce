using Microsoft.Extensions.DependencyInjection;
using MiniEcommerceCase.Application.Interfaces;
using MiniEcommerceCase.Application.Interfaces.Messaging;
using MiniEcommerceCase.Infrastructure.Messaging;
using MiniEcommerceCase.Infrastructure.Services;

namespace MiniEcommerceCase.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IEventPublisher, RabbitMqPublisher>();

            return services;
        }
    }
}
