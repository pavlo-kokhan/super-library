using Booking.Api.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SuperLibrary.Web.Helpers;

namespace Booking.Api.Extensions;

public static class ServiceCollectionRabbitMqExtensions
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration) 
        => services
            .Configure<RabbitMqOptions>(options =>
            {
                options.HostName = configuration["RabbitMq:Host"]!;
                options.Port = int.Parse(configuration["RabbitMq:Port"]!);
                options.UserName = configuration["RabbitMq:UserName"]!;
                options.Password = configuration["RabbitMq:Password"]!;
                options.ExchangeName = configuration["RabbitMq:Exchange"]!;;
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