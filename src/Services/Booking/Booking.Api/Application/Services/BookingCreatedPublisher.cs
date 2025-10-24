using System.Text;
using System.Text.Json;
using Booking.Api.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SuperLibrary.RabbitMQ.Abstractions;
using SuperLibrary.RabbitMQ.Constants;
using SuperLibrary.RabbitMQ.Models.Booking;

namespace Booking.Api.Application.Services;

public class BookingCreatedPublisher : IRabbitMqPublisher<BookingCreatedMessage>
{
    private readonly RabbitMqOptions _rabbitMqOptions;
    private readonly IConnectionFactory _factory;
    private readonly ILogger<BookingCreatedPublisher> _logger;

    public BookingCreatedPublisher(
        IOptions<RabbitMqOptions> rabbitMqOptions, 
        IConnectionFactory factory, 
        ILogger<BookingCreatedPublisher> logger)
    {
        _rabbitMqOptions = rabbitMqOptions.Value;
        _factory = factory;
        _logger = logger;
    }

    public async Task PublishAsync(BookingCreatedMessage message, CancellationToken cancellationToken)
    {
        await using var connection = await _factory.CreateConnectionAsync(cancellationToken);
        await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await channel.ExchangeDeclareAsync(
            _rabbitMqOptions.ExchangeName,
            ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            cancellationToken: cancellationToken);

        var jsonMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        await channel.BasicPublishAsync(
            exchange: _rabbitMqOptions.ExchangeName,
            routingKey: RoutingKeys.BookingCreated,
            body: body,
            cancellationToken: cancellationToken);
        
        _logger.LogInformation("Published message: {Message} to {Exchange}", message, _rabbitMqOptions.ExchangeName);
    }
}