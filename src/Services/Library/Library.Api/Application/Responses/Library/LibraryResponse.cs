using Library.Domain.AggregationRoots.ValueObjects;

namespace Library.Api.Application.Responses.Library;

public record LibraryResponse(int Id, string Name, AddressValueObject Address, WeekScheduleValueObject WeekSchedule);