using Microsoft.Extensions.Options;
using Notification.Api.Options;
using RabbitMQ.Client;
using SuperLibrary.Web.Helpers;

namespace Notification.Api.Extensions;

public static class ServiceCollectionRabbitMqExtensions
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration) 
        => services
            .Configure<RabbitMqOptions>(options =>
            {
                options.HostName = configuration["RabbitMQ:Host"]!;
                options.Port = int.Parse(configuration["RabbitMQ:Port"]!);
                options.UserName = configuration["RabbitMQ:UserName"]!;
                options.Password = configuration["RabbitMQ:Password"]!;
                options.BookingExchangeName = configuration["RabbitMQ:Exchange"]!;
                options.BookingsQueueName = configuration["RabbitMQ:Queue"]!;
                options.BookingsQueueRoutingKey = configuration["RabbitMQ:QueueRoutingKey"]!;
            })
            .AddSingleton<IConnectionFactory>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
                
                return new ConnectionFactory
                {
                    HostName = options.HostName,
                    Port = options.Port,
                    UserName = options.UserName,
                    Password = options.Password,
                    AutomaticRecoveryEnabled = true,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(5)
                };
            });
}