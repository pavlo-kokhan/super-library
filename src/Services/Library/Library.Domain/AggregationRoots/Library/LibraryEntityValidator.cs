using FluentValidation;
using Library.Domain.AggregationRoots.ValueObjects;

namespace Library.Domain.AggregationRoots.Library;

public class LibraryEntityValidator : AbstractValidator<LibraryEntity>
{
    public LibraryEntityValidator(IValidator<AddressValueObject> addressValidator, IValidator<WeekScheduleValueObject> weekScheduleValidator)
    {
        // RuleFor(x => x.Id).NotEmpty().Must(x => x >= 0);
        // RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        // RuleFor(x => x.LibrarianUserId).NotEmpty().Must(x => x >= 0);
        // RuleFor(x => x.Address).SetValidator(addressValidator);
        // RuleFor(x => x.WeekSchedule).SetValidator(weekScheduleValidator);
    }
}