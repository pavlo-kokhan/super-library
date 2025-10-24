using Booking.Api.Constants;

namespace Booking.Api.Extensions;

public static class HttpClientExtensions
{
    public static HttpClient CreateLibraryClient(this IHttpClientFactory factor) 
        => factor.CreateClient(HttpClientNames.Library);
}