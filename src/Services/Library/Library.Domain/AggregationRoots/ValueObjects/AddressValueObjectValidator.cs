using FluentValidation;
using SuperLibrary.Web.Attributes;

namespace Library.Domain.AggregationRoots.ValueObjects;

[ValueObjectValidator]
public class AddressValueObjectValidator : AbstractValidator<AddressValueObject>
{
    public AddressValueObjectValidator()
    {
        // RuleFor(x => x.Country).MaximumLength(100);
        // RuleFor(x => x.City).MaximumLength(100);
        // RuleFor(x => x.Street).MaximumLength(100);
        // RuleFor(x => x.HouseNumber).MaximumLength(100);
    }
}