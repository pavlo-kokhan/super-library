namespace Library.Domain.AggregationRoots.ValueObjects;

public record DayScheduleValueObject(DayOfWeek Day, TimeRangeValueObject WorkingHours)
{
    public static IReadOnlyCollection<DayScheduleValueObject> Defaults =>
    [
        new(DayOfWeek.Monday,    new(new(9, 0),  new(18, 0))),
        new(DayOfWeek.Tuesday,   new(new(9, 0),  new(18, 0))),
        new(DayOfWeek.Wednesday, new(new(9, 0),  new(18, 0))),
        new(DayOfWeek.Thursday,  new(new(9, 0),  new(18, 0))),
        new(DayOfWeek.Friday,    new(new(9, 0),  new(18, 0))),
        new(DayOfWeek.Saturday,  new(new(10, 0), new(16, 0))),
    ];
}