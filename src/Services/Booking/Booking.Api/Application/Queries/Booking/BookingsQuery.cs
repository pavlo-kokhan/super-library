using Booking.Api.Application.Responses;
using Booking.Api.Application.Services.Abstract;
using Booking.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SuperLibrary.Web.Results;
using SuperLibrary.Web.Results.Generic;
using SuperLibrary.Web.ValidationErrors;

namespace Booking.Api.Application.Queries.Booking;

public record BookingsQuery(int RoomId) : IRequest<Result<List<BookingResponse>>>
{
    public class Handler : IRequestHandler<BookingsQuery, Result<List<BookingResponse>>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILibraryService _libraryService;

        public Handler(ApplicationDbContext dbContext, ILibraryService libraryService)
        {
            _dbContext = dbContext;
            _libraryService = libraryService;
        }

        public async Task<Result<List<BookingResponse>>> Handle(BookingsQuery request, CancellationToken cancellationToken)
        {
            var room = await _libraryService.GetRoomByIdAsync(request.RoomId, cancellationToken);

            if (room is null)
                ValidationError.CreateInvalidArgument("ROOM_NOT_FOUND", ResultStatus.NotFound);

            var bookings = await _dbContext.NonDeletedBookings
                .Where(b => b.RoomId == request.RoomId)
                .Select(b =>
                    new BookingResponse(
                        b.Id, 
                        b.From, 
                        b.To, 
                        new RoomResponse(
                            room!.Id, 
                            room.Number, 
                            room.IsAvailable)))
                .ToListAsync(cancellationToken);

            return bookings;
        }
    }
}