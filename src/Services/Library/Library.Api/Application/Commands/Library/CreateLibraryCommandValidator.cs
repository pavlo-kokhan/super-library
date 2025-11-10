using FluentValidation;

namespace Library.Api.Application.Commands.Library;

public class CreateLibraryCommandValidator : AbstractValidator<CreateLibraryCommand>
{
    public CreateLibraryCommandValidator()
    {
        
    }
}