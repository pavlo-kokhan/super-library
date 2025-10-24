namespace Booking.Api.Options;

public class RabbitMqOptions
{
    public string HostName { get; set; } = null!;
    
    public int Port { get; set; }
    
    public string UserName { get; set; } = null!;
    
    public string Password { get; set; } = null!;
    
    public string ExchangeName { get; set; } = null!;
}