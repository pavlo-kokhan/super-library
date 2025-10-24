using SuperLibrary.Web.Responses.LibraryApi.ValueObjectResponses;

namespace SuperLibrary.Web.Responses.LibraryApi.Library;

public record LibraryResponseDto(int Id, string Name, AddressResponseDto Address, WeekScheduleResponseDto WeekSchedule);