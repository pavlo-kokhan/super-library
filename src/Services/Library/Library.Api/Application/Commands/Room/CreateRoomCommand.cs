using Library.Domain.AggregationRoots.Room;
using Library.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SuperLibrary.Web.Results;
using SuperLibrary.Web.ValidationErrors;

namespace Library.Api.Application.Commands.Room;

public record CreateRoomCommand(int Number, bool IsAvailable, int LibraryId) : IRequest<Result>
{
    public class Handler : IRequestHandler<CreateRoomCommand, Result>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext) 
            => _dbContext = dbContext;

        public async Task<Result> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            var library = await _dbContext.NonDeletedLibraries
                .Where(l => l.Id == request.LibraryId)
                .FirstOrDefaultAsync(cancellationToken);

            if (library is null)
                return ValidationError.CreateInvalidArgument("LIBRARY_NOT_FOUND", ResultStatus.NotFound);

            var room = RoomEntity.Create(request.LibraryId, request.Number, request.IsAvailable);

            if (room.IsFailure)
                return room;

            await _dbContext.AddAsync(room.Data, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}