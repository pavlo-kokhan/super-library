using Microsoft.AspNetCore.Mvc;
using SuperLibrary.RabbitMQ.Models.Booking;

namespace Notification.Api.Controllers;

[ApiController]
[Route("notifications")]
public class NotificationController : ControllerBase
{
    private readonly ILogger<NotificationController> _logger;

    public NotificationController(ILogger<NotificationController> logger)
    {
        _logger = logger;
    }

    [HttpPost("booking-created")]
    public Task<IActionResult> NotifyBookingCreatedAsync(BookingCreatedMessage message)
    {
        _logger.LogInformation("Received message (synchronous approach): {Message}", message);

        return Task.FromResult<IActionResult>(Ok());
    }
}