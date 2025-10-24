using Booking.Api.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SuperLibrary.Web.Helpers;

namespace Booking.Api.Extensions;

public static class ServiceCollectionRabbitMqExtensions
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services) 
        => services
            .Configure<RabbitMqOptions>(options =>
            {
                options.HostName = DotNetEnvHelper.GetEnvironmentVariableOrThrow("RABBITMQ_HOST");
                options.Port = int.Parse(DotNetEnvHelper.GetEnvironmentVariableOrThrow("RABBITMQ_PORT"));
                options.UserName = DotNetEnvHelper.GetEnvironmentVariableOrThrow("RABBITMQ_USERNAME");
                options.Password = DotNetEnvHelper.GetEnvironmentVariableOrThrow("RABBITMQ_PASSWORD");
                options.ExchangeName = DotNetEnvHelper.GetEnvironmentVariableOrThrow("RABBITMQ_EXCHANGE");
            })
            .AddSingleton<IConnectionFactory>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
                
                return new ConnectionFactory
                {
                    HostName = options.HostName,
                    Port = options.Port,
                    UserName = options.UserName,
                    Password = options.Password
                };
            });
}