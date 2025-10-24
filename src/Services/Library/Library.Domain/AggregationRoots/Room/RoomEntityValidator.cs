using FluentValidation;

namespace Library.Domain.AggregationRoots.Room;

public class RoomEntityValidator : AbstractValidator<RoomEntity>
{
    public RoomEntityValidator()
    {
        // RuleFor(x => x.LibraryId).NotEmpty();
        // RuleFor(x => x.Number).NotEmpty();
        // RuleFor(x => x.IsAvailable).NotEmpty();
    }
}