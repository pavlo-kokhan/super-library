using FluentValidation;

namespace Library.Api.Application.Queries.Library;

public class LibraryQueryValidator : AbstractValidator<LibraryQuery>
{
    public LibraryQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().Must(x => x > 0);
    }
}