using System.Reflection;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Messaging.MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(r =>
        {   
            r.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            r.AddOpenBehavior(typeof(ValidationBehavior<,>));
            r.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });
        
        services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());
        return services;
    }
}