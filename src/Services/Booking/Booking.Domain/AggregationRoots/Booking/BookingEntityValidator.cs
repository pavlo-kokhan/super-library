using FluentValidation;

namespace Booking.Domain.AggregationRoots.Booking;

public class BookingEntityValidator : AbstractValidator<BookingEntity>
{
    public BookingEntityValidator()
    {
        // RuleFor(x => x.From).NotEmpty().Must(x => x > DateTime.UtcNow);
        // RuleFor(x => x.To).NotEmpty().Must(x => x > DateTime.UtcNow);
        // RuleFor(x => x).Must(x => x.From < x.To);
    }
}