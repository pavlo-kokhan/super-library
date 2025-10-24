namespace Notification.Api.Options;

public class RabbitMqOptions
{
    public string HostName { get; set; } = string.Empty;
    public int Port { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string BookingCreatedExchangeName { get; set; } = string.Empty;
    public string BookingsQueueName { get; set; } = string.Empty;
}