using Booking.Api.Constants;

namespace Booking.Api.Extensions;

public static class HttpClientFactoryExtensions
{
    public static HttpClient CreateLibraryClient(this IHttpClientFactory factory) 
        => factory.CreateClient(HttpClientNames.Library);
    
    public static HttpClient CreateNotificationClient(this IHttpClientFactory factory)
        => factory.CreateClient(HttpClientNames.Notification);
}