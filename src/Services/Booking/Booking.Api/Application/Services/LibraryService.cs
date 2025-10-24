using Booking.Api.Application.Services.Abstract;
using Booking.Api.Extensions;
using SuperLibrary.Web.Responses.LibraryApi.Room;

namespace Booking.Api.Application.Services;

public class LibraryService : ILibraryService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public LibraryService(IHttpClientFactory httpClientFactory) 
        => _httpClientFactory = httpClientFactory;

    public async Task<RoomResponseDto?> GetRoomByIdAsync(int roomId, CancellationToken cancellationToken = default)
    {
        var client = _httpClientFactory.CreateLibraryClient();

        var response = await client.GetFromJsonAsync<RoomResponseDto>($"rooms/{roomId}", cancellationToken);

        return response;
    }
}