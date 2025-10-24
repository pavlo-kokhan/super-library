using FluentValidation;
using SuperLibrary.Web.Attributes;

namespace Library.Domain.AggregationRoots.ValueObjects;

[ValueObjectValidator]
public class WeekScheduleValueObjectValidator : AbstractValidator<WeekScheduleValueObject>
{
    private static readonly DayScheduleValueObjectValidator DayScheduleValueObjectValidator = new();
    
    public WeekScheduleValueObjectValidator()
    {
        // RuleFor(x => x.ClosedDates).NotEmpty();
        // RuleFor(x => x.Days)
        //     .NotEmpty()
        //     .ForEach(x => x.SetValidator(DayScheduleValueObjectValidator));
    }
}