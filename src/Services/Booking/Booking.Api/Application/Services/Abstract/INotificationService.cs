using SuperLibrary.RabbitMQ.Models.Booking;
using SuperLibrary.Web.Results;

namespace Booking.Api.Application.Services.Abstract;

public interface INotificationService
{
    public Task<Result> NotifyBookingCreatedAsync(BookingCreatedMessage message, CancellationToken cancellationToken = default);
}