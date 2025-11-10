using System.Text;
using Microsoft.Extensions.Options;
using Notification.Api.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SuperLibrary.RabbitMQ.Constants;

namespace Notification.Api.BackgroundServices;

public class BookingConsumer : BackgroundService
{
    private readonly RabbitMqOptions _rabbitMqOptions;
    private readonly IConnectionFactory _factory;
    private readonly ILogger<BookingConsumer> _logger;

    public BookingConsumer(
        IOptions<RabbitMqOptions> rabbitMqOptions, 
        IConnectionFactory factory, 
        ILogger<BookingConsumer> logger)
    {
        _rabbitMqOptions = rabbitMqOptions.Value;
        _factory = factory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var connection = await _factory.CreateConnectionAsync(stoppingToken);
        await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);
        
        await channel.ExchangeDeclareAsync(
            exchange: _rabbitMqOptions.BookingExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            cancellationToken: stoppingToken);

        await channel.QueueDeclareAsync(
            queue: _rabbitMqOptions.BookingsQueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: stoppingToken);

        await channel.QueueBindAsync(
            queue: _rabbitMqOptions.BookingsQueueName,
            exchange: _rabbitMqOptions.BookingExchangeName,
            routingKey: _rabbitMqOptions.BookingsQueueRoutingKey,
            cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            
            _logger.LogInformation("Received message: {Message}", message);

            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(
            _rabbitMqOptions.BookingsQueueName,
            autoAck: false,
            consumer,
            cancellationToken: stoppingToken);
        
        _logger.LogInformation(
            "{Consumer} started executing. Listening {Queue} on {Exchange} rk: {RoutingKey}",
            nameof(BookingConsumer),
            _rabbitMqOptions.BookingsQueueName,
            _rabbitMqOptions.BookingExchangeName,
            RoutingKeys.BookingCreated);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}