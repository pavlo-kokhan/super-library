using FluentValidation;
using SuperLibrary.Web.Attributes;

namespace Library.Domain.AggregationRoots.ValueObjects;

[ValueObjectValidator]
public class DayScheduleValueObjectValidator : AbstractValidator<DayScheduleValueObject>
{
    private static readonly TimeRangeValueObjectValidator TimeRangeValueObjectValidator = new();
    
    public DayScheduleValueObjectValidator()
    {
        // RuleFor(x => x.Day).NotEmpty();
        // RuleFor(x => x.WorkingHours).NotEmpty().SetValidator(TimeRangeValueObjectValidator);
    }
}