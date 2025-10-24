namespace Library.Domain.AggregationRoots.ValueObjects;

public record TimeRangeValueObject(TimeOnly From, TimeOnly To)
{
    public bool IsOverlapped(TimeRangeValueObject other) => From <= other.To && other.From <= To;
}