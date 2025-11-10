using Booking.Api.Application.Services.Abstract;
using Booking.Api.Extensions;
using SuperLibrary.RabbitMQ.Models.Booking;
using SuperLibrary.Web.Results;
using SuperLibrary.Web.ValidationErrors;

namespace Booking.Api.Application.Services;

public class NotificationService : INotificationService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public NotificationService(IHttpClientFactory httpClientFactory) 
        => _httpClientFactory = httpClientFactory;
    
    public async Task<Result> NotifyBookingCreatedAsync(BookingCreatedMessage message, CancellationToken cancellationToken = default)
    {
        var client = _httpClientFactory.CreateNotificationClient();

        var response = await client.PostAsJsonAsync("notifications/booking-created", message, cancellationToken);

        return response.IsSuccessStatusCode ? Result.Success() : ValidationError.CreateInvalidArgument("NOTIFICATION_FAILED");
    }
}