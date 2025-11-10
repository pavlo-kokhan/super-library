using FluentValidation;

namespace Library.Api.Application.Commands.Room;

public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        
    }
}