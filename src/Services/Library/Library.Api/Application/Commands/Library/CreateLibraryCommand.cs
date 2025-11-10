using Library.Domain.AggregationRoots.Library;
using Library.Domain.AggregationRoots.ValueObjects;
using Library.Infrastructure.Persistence;
using MediatR;
using SuperLibrary.Web.Results;

namespace Library.Api.Application.Commands.Library;

public record CreateLibraryCommand(string Name, AddressValueObject Address, WeekScheduleValueObject WeekSchedule) : IRequest<Result>
{
    public class Handler : IRequestHandler<CreateLibraryCommand, Result>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext) 
            => _dbContext = dbContext;

        public async Task<Result> Handle(CreateLibraryCommand request, CancellationToken cancellationToken)
        {
            var createLibraryResult = LibraryEntity.Create(request.Name, request.Address, request.WeekSchedule, 1);
            
            if (createLibraryResult.IsFailure)
                return createLibraryResult;
            
            await _dbContext.AddAsync(createLibraryResult.Data, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            return Result.Success();
        }
    }
}