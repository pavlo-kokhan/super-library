using Library.Api.Application.Responses.Library;
using Library.Api.Application.Responses.Room;
using Library.Infrastructure.Persistence;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SuperLibrary.Web.Results;
using SuperLibrary.Web.Results.Generic;
using SuperLibrary.Web.ValidationErrors;

namespace Library.Api.Application.Queries.Room;

public record RoomQuery(int Id) : IRequest<Result<RoomResponse>>
{
    public class Handler : IRequestHandler<RoomQuery, Result<RoomResponse>>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext) 
            => _dbContext = dbContext;
        
        public async Task<Result<RoomResponse>> Handle(RoomQuery request, CancellationToken cancellationToken)
        {
            var room = await _dbContext.NonDeletedRooms
                .Where(r => r.Id == request.Id)
                .LeftJoin(
                    _dbContext.NonDeletedLibraries,
                    r => r.LibraryId,
                    l => l.Id,
                    (r, l) => new RoomResponse(
                        r.Id,
                        r.Number,
                        r.IsAvailable,
                        new ShortLibraryResponse(l.Id, l.Name)))
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (room is null)
                return ValidationError.CreateInvalidArgument("ROOM_NOT_FOUND", ResultStatus.NotFound);

            return room;
        }
    }
}