namespace SuperLibrary.Web.Responses.LibraryApi.ValueObjectResponses;

public record DayScheduleResponseDto(DayOfWeek Day, TimeRangeResponseDto WorkingHours);