using Booking.Api.Application.Services.Abstract;
using Booking.Domain.AggregationRoots.Booking;
using Booking.Infrastructure.Persistence;
using MediatR;
using SuperLibrary.RabbitMQ.Abstractions;
using SuperLibrary.RabbitMQ.Models.Booking;
using SuperLibrary.Web.Results;
using SuperLibrary.Web.ValidationErrors;

namespace Booking.Api.Application.Commands.Booking;

public record CreateBookingCommand(DateTime From, DateTime To, int RoomId) : IRequest<Result>
{
    public class Handler : IRequestHandler<CreateBookingCommand, Result>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILibraryService _libraryService;
        private readonly IRabbitMqPublisher<BookingCreatedMessage> _publisher;

        public Handler(ApplicationDbContext dbContext, ILibraryService libraryService, IRabbitMqPublisher<BookingCreatedMessage> publisher)
        {
            _dbContext = dbContext;
            _libraryService = libraryService;
            _publisher = publisher;
        }

        public async Task<Result> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var room = await _libraryService.GetRoomByIdAsync(request.RoomId, cancellationToken);

            if (room is null)
                ValidationError.CreateInvalidArgument("ROOM_NOT_FOUND", ResultStatus.NotFound);

            var createBookingResult = BookingEntity.Create(request.From, request.To, request.RoomId);

            if (createBookingResult.IsFailure)
                return createBookingResult;

            await _dbContext.AddAsync(createBookingResult.Data, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var booking = createBookingResult.Data;
            await _publisher.PublishAsync(new BookingCreatedMessage(booking.From, booking.To, booking.RoomId), cancellationToken);

            return Result.Success();
        }
    }
}