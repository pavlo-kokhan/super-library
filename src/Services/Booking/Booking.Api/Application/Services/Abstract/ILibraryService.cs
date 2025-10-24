using SuperLibrary.Web.Responses.LibraryApi.Room;
using SuperLibrary.Web.Results.Generic;

namespace Booking.Api.Application.Services.Abstract;

public interface ILibraryService
{
    public Task<RoomResponseDto?> GetRoomByIdAsync(int roomId, CancellationToken cancellationToken = default);
}