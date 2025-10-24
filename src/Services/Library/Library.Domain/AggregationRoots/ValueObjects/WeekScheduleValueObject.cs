namespace Library.Domain.AggregationRoots.ValueObjects;

public record WeekScheduleValueObject(
    IReadOnlyCollection<DayScheduleValueObject> Days,
    IReadOnlyCollection<DateOnly> ClosedDates)
{
    public static WeekScheduleValueObject Defaults => new(DayScheduleValueObject.Defaults, []);
}