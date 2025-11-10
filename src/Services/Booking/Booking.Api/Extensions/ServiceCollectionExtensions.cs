using Booking.Api.Application.Services.Publishers;
using SuperLibrary.RabbitMQ.Abstractions;
using SuperLibrary.RabbitMQ.Models.Booking;

namespace Booking.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPublishers(this IServiceCollection services) 
        => services
            .AddScoped<IRabbitMqPublisher<BookingCreatedMessage>, BookingCreatedPublisher>()
            .AddScoped<IRabbitMqPublisher<BookingUpdatedMessage>, BookingUpdatedPublisher>()
            .AddScoped<IRabbitMqPublisher<BookingDeletedMessage>, BookingDeletedPublisher>();
}