using SuperLibrary.Web.Responses.LibraryApi.Library;

namespace SuperLibrary.Web.Responses.LibraryApi.Room;

public record RoomResponseDto(int Id, int Number, bool IsAvailable, ShortLibraryResponseDto Library);