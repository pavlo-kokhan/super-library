using FluentValidation;
using SuperLibrary.Web.Attributes;

namespace Library.Domain.AggregationRoots.ValueObjects;

[ValueObjectValidator]
public class TimeRangeValueObjectValidator : AbstractValidator<TimeRangeValueObject>
{
    public TimeRangeValueObjectValidator()
    {
        // RuleFor(x => x).Must(x => x.From < x.To);
        // RuleFor(x => x.From)
        //     .Must(x => x >= TimeOnly.MinValue && x <= new TimeOnly(23, 59, 59));
        // RuleFor(x => x.To)
        //     .Must(x => x > TimeOnly.MinValue && x <= new TimeOnly(23, 59, 59));
    }
}