using Library.Api.Application.Responses.Library;
using Library.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SuperLibrary.Web.Results;
using SuperLibrary.Web.Results.Generic;
using SuperLibrary.Web.ValidationErrors;

namespace Library.Api.Application.Queries.Library;

public record LibraryQuery(int Id) : IRequest<Result<LibraryResponse>>
{
    public class Handler : IRequestHandler<LibraryQuery, Result<LibraryResponse>>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext) 
            => _dbContext = dbContext;

        public async Task<Result<LibraryResponse>> Handle(LibraryQuery request, CancellationToken cancellationToken)
        {
            var library = await _dbContext.NonDeletedLibraries
                .Where(l => l.Id == request.Id)
                .Select(l => new LibraryResponse(l.Id, l.Name, l.Address, l.WeekSchedule))
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (library is null)
                return ValidationError.CreateInvalidArgument("LIBRARY_NOT_FOUND", ResultStatus.NotFound);

            return library;
        }
    }
}