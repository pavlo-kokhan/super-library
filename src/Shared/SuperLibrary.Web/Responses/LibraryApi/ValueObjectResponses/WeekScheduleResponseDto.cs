namespace SuperLibrary.Web.Responses.LibraryApi.ValueObjectResponses;

public record WeekScheduleResponseDto(
    IReadOnlyCollection<DayScheduleResponseDto> Days,
    IReadOnlyCollection<DateOnly> ClosedDates);