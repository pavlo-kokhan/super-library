using Library.Api.Application.Responses.Library;

namespace Library.Api.Application.Responses.Room;

public record RoomResponse(int Id, int Number, bool IsAvailable, ShortLibraryResponse Library);