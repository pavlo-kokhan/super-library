using Booking.Api.Application.Services.Abstract;
using Booking.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SuperLibrary.RabbitMQ.Abstractions;
using SuperLibrary.RabbitMQ.Models.Booking;
using SuperLibrary.Web.Results;
using SuperLibrary.Web.ValidationErrors;

namespace Booking.Api.Application.Commands.Booking;

public record UpdateBookingCommand(int Id, DateTime From, DateTime To, int RoomId) : IRequest<Result>
{
    public class Handler : IRequestHandler<UpdateBookingCommand, Result>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILibraryService _libraryService;
        private readonly IRabbitMqPublisher<BookingUpdatedMessage> _publisher;

        public Handler(
            ApplicationDbContext dbContext, 
            ILibraryService libraryService, 
            IRabbitMqPublisher<BookingUpdatedMessage> publisher)
        {
            _dbContext = dbContext;
            _libraryService = libraryService;
            _publisher = publisher;
        }

        public async Task<Result> Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
        {
            var room = await _libraryService.GetRoomByIdAsync(request.RoomId, cancellationToken);

            if (room is null)
                return ValidationError.CreateInvalidArgument("ROOM_NOT_FOUND", ResultStatus.NotFound);

            var booking = await _dbContext.NonDeletedBookings
                .Where(b => b.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (booking is null)
                return ValidationError.CreateInvalidArgument("BOOKING_NOT_FOUND", ResultStatus.NotFound);
            
            var updateBookingResult = booking.Update(request.From, request.To, request.RoomId);

            if (updateBookingResult.IsFailure)
                return updateBookingResult;
            
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            await _publisher.PublishAsync(
                new BookingUpdatedMessage(
                    request.Id, 
                    request.From, 
                    request.To, 
                    request.RoomId,
                    DateTime.UtcNow), 
                cancellationToken);
            
            return Result.Success();
        }
    }
}