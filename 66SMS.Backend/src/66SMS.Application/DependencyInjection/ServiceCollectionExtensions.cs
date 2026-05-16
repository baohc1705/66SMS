using _66SMS.Application.Abstractions.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace _66SMS.Application.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(ServiceCollectionExtensions).Assembly;

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}
