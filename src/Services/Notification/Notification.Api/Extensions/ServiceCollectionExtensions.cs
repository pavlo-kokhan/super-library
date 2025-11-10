using Notification.Api.BackgroundServices;

namespace Notification.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsumers(this IServiceCollection services) 
        => services.AddHostedService<BookingConsumer>();
}